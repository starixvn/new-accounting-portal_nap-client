using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Context;
using Fanex.NAP.Core.Models.ValueObject;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.SignIn.Models;
using Fanex.NAP.Core.SignIn.ViewModel;
using Fanex.NAP.Core.Web.Auth;
using Fanex.NAP.Core.Web.Session;
using ONEbook.UM;
using ONEbook.UM.Services;

namespace Fanex.NAP.Core.SignIn
{
    using Core.Models;
    using Core.Models.ParameterObject;
    using Web;

    public class AuthController : Controller
    {
        #region members

        private const string FormTokenName = "__SecureFormToken";
        private readonly CurrentSession currentSession;
        private readonly SecurityService securityService;
        private readonly GroupService groupService;
        private readonly UserService userService;
        private readonly CurrentContext currentContext;
        private readonly IMaintenanceService maintenanceService;

        #endregion members

        #region ctors

        public AuthController()
        {
            userService = new UserService();
            groupService = new GroupService();
            currentSession = new CurrentSession();
            currentContext = new CurrentContext();
            securityService = new SecurityService();
            maintenanceService = new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext);
        }

        #endregion ctors

        #region sign in

        public string FormToken
        {
            get
            {
                if (Session[FormTokenName] == null)
                {
                    return string.Empty;
                }

                return Session[FormTokenName].ToString();
            }

            set
            {
                Session[FormTokenName] = value;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            GenerateNewToken();

            var indexViewModel = new IndexViewModel();
            indexViewModel.FormToken = FormToken;

            if (!NapSettings.Settings.IsNapHub && !NapSettings.Settings.EnableDirectLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            return View("Index", indexViewModel);
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(string username, string password, string loginToken)
        {
            var indexViewModel = new IndexViewModel();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = userService.Get(username.Trim());
                if (user != null && CheckValidToken(loginToken))
                {
                    string hashPassword = SHA256(user.Password + loginToken);
                    if (password == hashPassword)
                    {
                        ClearToken();
                        var group = user.GroupId == 0 ? null : groupService.Get(user.GroupId);

                        var siteLevelList = userService.GetSiteLevel(user.Id, NapSettings.Settings.Environment);

                        if ((group == null || group.IsActive) &&
                            siteLevelList.Any(x => x.IsActive))
                        {
                            var criteria = SetUserCheckInCriteria(user.Id);
                            var onUserId = securityService.Login(criteria);


                            SaveUserSession(user, username, onUserId);

                            if (UserIsActiveOnThisSite(siteLevelList))
                            {
                                if (!string.IsNullOrEmpty(PropagationAuthUrl))
                                {
                                    return RedirectToAspNetCore(username, password, loginToken);
                                }
                                else
                                {
                                    return Redirect(FormsAuthentication.DefaultUrl);
                                }
                            }
                            else
                            {
                                DoSignOut();
                                indexViewModel.Message = ViewResources.Your_account_was_suspended;
                            }
                        }
                        else
                        {
                            indexViewModel.Message = siteLevelList.Any() ? ViewResources.Your_account_was_suspended : ViewResources.You_have_no_authorization;
                        }
                    }
                    else
                    {
                        indexViewModel.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    indexViewModel.Message = ViewResources.Invalid_Username_or_Password;
                }
            }
            else
            {
                indexViewModel.Message = "Invalid input!";
            }

            GenerateNewToken();
            indexViewModel.FormToken = FormToken;

            return View("Index", indexViewModel);
        }

        private ActionResult RedirectToAspNetCore(string username, string password, string loginToken)
        {
            var redirectUrl = Helper.GetAbsoluteRedirectUrl(Request, Url.Content(FormsAuthentication.DefaultUrl));
            var encodedRedirectUrl = HttpUtility.UrlEncode(redirectUrl);

            var redirectAuthUrl = PropagationAuthUrl
                + $"/Login?username={username}"
                + $"&password={password}"
                + $"&loginToken={loginToken}"
                + $"&redirectUrl={encodedRedirectUrl}";

            return Redirect(redirectAuthUrl);
        }

        #endregion sign in

        #region sign out

        [Authorize]
        public ActionResult LogOut()
        {
            DoSignOut();

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NetCoreAuth"]))
            {
                var redirectUrl = Helper.GetAbsoluteRedirectUrl(Request, Url.Content(FormsAuthentication.LoginUrl));
                var encodedRedirectUrl = HttpUtility.UrlEncode(redirectUrl);

                return Redirect(ConfigurationManager.AppSettings["NetCoreAuth"] + $"/LogOut?redirectUrl={encodedRedirectUrl}");
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion sign out

        #region re check in

        [AllowAnonymous]
        public ActionResult ReCheckIn()
        {
            if (!NapSettings.Settings.EnableReCheckIn)
            {
                return Json(new
                {
                    IsKickedOut = 0
                });
            }

            var isKickedOut = currentSession.OnUserId <= 0 || securityService.ReCheckIn(currentSession.OnUserId, NapSettings.Settings.SiteId) > 0 ? 1 : 0;

            return Json(new
            {
                OnUserId = currentSession.OnUserId,
                IsKickedOut = isKickedOut
            });
        }

        #endregion re check in

        #region helpers

        private void DoSignOut()
        {
            if (currentSession.OnUserId != 0)
            {
                securityService.Logout(currentSession.OnUserId);
            }

            FormsAuthentication.SignOut();
            currentSession.ClearAll();
        }

        private void CreateUserMenu(User user)
        {
            currentSession.MenuItemList = userService.GetMenuItem(
                user.Id,
                NapSettings.Settings.SiteId,
                NapSettings.Settings.Environment,
                true)
                .ToList();
        }

        private void CreateAuthInfo(User user)
        {
            var authInfo = new AuthInfo { UserId = user.Id, OnUserId = currentSession.OnUserId };
            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                false,
                authInfo.ToXml(),
                "/");

            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = "/",
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        private LoginParameter SetUserCheckInCriteria(int userId)
        {
            return new LoginParameter
            {
                UserId = userId,
                SiteId = NapSettings.Settings.SiteId,
                NetSessionId = Session.SessionID,
                Ip = currentContext.IP,
                LoginDate = DateTime.Now,
                BrowserName = currentContext.BrowserName,
                CountryCode = currentContext.CountryCode,
                Domain = currentContext.DomainName,
                Platform = currentContext.Platform
            };
        }

        private void SaveUserSession(User user, string userName, int onUserId)
        {
            currentSession.Level = userService.GetUserLevel(user.Id, NapSettings.Settings.SiteId);
            currentSession.UserId = user.Id;
            currentSession.Username = userName;
            currentSession.OnUserId = onUserId;
            currentSession.SessionId = Session.SessionID;

            if (NapSettings.Settings.SiteId != 0)
            {
                CreateUserMenu(user);
            }

            CreateAuthInfo(user);
        }

        private bool UserIsActiveOnThisSite(IEnumerable<SiteLevel> siteLevelList)
        {
            bool userIsActiveOnThisSite = false;

            foreach (var siteLevel in siteLevelList)
            {
                if (siteLevel.SiteId == NapSettings.Settings.SiteId && siteLevel.IsActive)
                {
                    userIsActiveOnThisSite = true;
                }
            }

            return userIsActiveOnThisSite;
        }

        private void AddTokenToCookie(string loginToken)
        {
            var responseCookie = new HttpCookie(FormTokenName)
            {
                HttpOnly = true,

                Value = loginToken
            };

            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            Response.Cookies.Set(responseCookie);
        }

        private bool CheckValidToken(string loginToken)
        {
            return !string.IsNullOrWhiteSpace(loginToken)
                && FormToken == loginToken
                && Request.Cookies[FormTokenName] != null
                && loginToken == Request.Cookies[FormTokenName].Value;
        }

        private void GenerateNewToken()
        {
            string loginToken = Guid.NewGuid().ToString("N");
            FormToken = loginToken;
            AddTokenToCookie(loginToken);
        }

        private string SHA256(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Hey guy! I dont want to hash an empty or white space string");
            }

            var hashAlgorithm = new SHA256Managed();
            var hashData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            string encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLower();

            return encrypted;
        }

        private void ClearToken()
        {
            Session.Remove(FormTokenName);

            if (Request.Cookies[FormTokenName] != null)
            {
                var cookie = new HttpCookie(FormTokenName) { Expires = DateTime.Now.AddDays(-1d) };
                Response.Cookies.Add(cookie);
            }
        }

        #endregion helpers

        #region common

        [AllowAnonymous]
        public ActionResult Offline()
        {
            var isDownTime = maintenanceService.IsUnderMaintenance();
            if (!isDownTime)
            {
                return Redirect("~");
            }

            var downTime = maintenanceService.GetUnderMaintenanceTime();

            var viewModel = new MaintenanceViewModel { UmtFrom = downTime.From, UmtTo = downTime.To };

            return View("Offline", viewModel);
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View("Error");
        }

        [AllowAnonymous]
        public ActionResult Static(string url)
        {
            var contentType = "application/octet-stream";
            if (url.EndsWith("css"))
            {
                contentType = "text/css";
            }
            else if (url.EndsWith("js"))
            {
                contentType = "text/javascript";
            }

            var resourceName = $"Fanex.NAP.Core.SignIn.Resources.{url}";
            var text = ReadResourceContent(resourceName);
            return Content(text, contentType);
        }

        [AllowAnonymous]
        public ActionResult RecheckinScript()
        {
            if (!NapSettings.Settings.EnableReCheckIn)
            {
                return Content(string.Empty, "text/javascript");
            }

            var resourceName = $"Fanex.NAP.Core.SignIn.Resources.recheckin.js";
            var text = ReadResourceContent(resourceName);
            var rootUrl = Url.Content("~/");

            return Content($"window.rootUrl='{rootUrl}';\r\n{text}", "text/javascript");
        }


        private string ReadResourceContent(string resourceName)
        {
            var assembly = typeof(AuthController).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string PropagationAuthUrl
        {
            get
            {
                var legacyPropagationAuth = ConfigurationManager.AppSettings["NetCoreAuth"] ?? string.Empty;
                var propagationAuth = ConfigurationManager.AppSettings["PropagationAuth"] ?? string.Empty;

                propagationAuth = string.IsNullOrEmpty(propagationAuth) ? legacyPropagationAuth : propagationAuth;
                propagationAuth = propagationAuth.Trim().TrimEnd('/');

                return propagationAuth;
            }
        }

        #endregion

        #region Password

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View("ChangePassword", new UpdatePasswordModel());
        }

        [HttpPost]
        [Authorize]
        public JsonResult ChangePassword(UpdatePasswordModel criteria)
        {
            var user = userService.Get(currentSession.UserId);
            var oldPwd = criteria.OldUserPwd;

            if (oldPwd.Equals(user.Password))
            {
                var password = criteria.UserPwd;
                securityService.UpdatePassword(NapSettings.Settings.SiteId, user.Id, password);
            }
            else
            {
                return Json(new { ErrorMsg = ViewResources.Wrong_password });
            }

            return Json(new { ErrorMsg = "" });
        }


        #endregion

        [AllowAnonymous]
        public ActionResult WhoAmI()
        {
            return Content("OK");
        }
    }
}
