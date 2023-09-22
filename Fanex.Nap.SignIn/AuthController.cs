namespace Fanex.Nap.SignIn
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Session;
    using Models;
    using NAP.Core.Web.Auth;
    using NAP.Core.Web.Context;
    using ONEbook.UM;
    using ONEbook.UM.Services;
    using ViewModel;

    [AllowAnonymous]
    public class AuthController : Controller
    {
        #region members

        private const string FormTokenName = "__SecureFormToken";
        private const string OtpSite = "NAP";
        private readonly CurrentSession currentSession;
        private readonly SecurityService securityService;
        private readonly GroupService groupService;
        private readonly UserService userService;
        private readonly CurrentContext currentContext;
        private readonly IMaintenanceService maintenanceService;
        private readonly SSOService ssoService;

        private readonly string propagationAuthUrl = Helper.GetPropagationAuthUrl();

        private readonly string homeUrl = Helper.GetHomeUrl();

        #endregion members

        #region ctors

        public AuthController()
        {
            userService = new UserService();
            groupService = new GroupService();
            currentSession = new CurrentSession();
            currentContext = new CurrentContext();
            securityService = new SecurityService();
            ssoService = new SSOServiceImp();
            maintenanceService = new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext);
        }

        #endregion ctors

        #region sign in

        private string FormToken
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

        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.EnableDirectLogin)
            {
                return Redirect(NapSettings.Settings.HubUrl);
            }

            Session["OtpUserId"] = 0;
            GenerateNewToken();
            var indexViewModel = new IndexViewModel { FormToken = FormToken };
            return View("Index", indexViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(string username, string password, string loginToken)
        {
            return NapSettings.Settings.IsNapHub
                ? IndexNapHub(username, password, loginToken)
                : IndexForSitesOtherThanNapHub(username, password, loginToken);
        }


        private ActionResult IndexNapHub(string username, string password, string loginToken)
        {
            var indexViewModel = new IndexViewModel();
            if (IsValidUserPassword(username, password, loginToken))
            {
                var user = userService.Get(username.Trim());
                return LoginFlow(user);
            }
            else
            {
                indexViewModel.Message = ViewResources.Invalid_Username_or_Password;
            }

            GenerateNewToken();
            indexViewModel.FormToken = FormToken;

            return View("Index", indexViewModel);
        }

        public ActionResult IndexForSitesOtherThanNapHub(string username, string password, string loginToken)
        {
            var indexViewModel = new IndexViewModel();

            if (IsValidUserPassword(username, password, loginToken))
            {
                var user = userService.Get(username.Trim());

                if (IsAuthorizedUser(user))
                {
                    if (IsActivatedUser(user))
                    {
                        return LoginFlow(user);
                    }
                    else
                    {
                        indexViewModel.Message = ViewResources.Your_account_was_suspended;
                    }
                }
                else
                {
                    indexViewModel.Message = ViewResources.You_have_no_authorization;
                }
            }
            else
            {
                indexViewModel.Message = ViewResources.Invalid_Username_or_Password;
            }

            GenerateNewToken();
            indexViewModel.FormToken = FormToken;

            return View("Index", indexViewModel);
        }

        private bool IsValidUserPassword(string username, string password, string token)
        {
            if (!IsValidToken(token)
                || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = userService.Get(username.Trim());

            if (user == null || user.Id == 0)
            {
                return false;
            }

            string hashPassword = SHA256(user.Password + token);
            if (password != hashPassword)
            {
                return false;
            }

            return true;
        }


        private bool IsAuthorizedUser(User user)
        {
            var group = user.GroupId == 0 ? null : groupService.Get(user.GroupId);

            var siteLevelList = userService.GetSiteLevel(user.Id, NapSettings.Settings.Environment);

            return (group == null || group.IsActive) &&
                 siteLevelList.Any(x => x.IsActive);
        }

        private bool IsActivatedUser(User user)
        {
            var siteLevelList = userService.GetSiteLevel(user.Id, NapSettings.Settings.Environment);

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

        #endregion sign in

        #region sign out

        [Authorize]
        public ActionResult LogOut()
        {
            DoSignOut();

            if (string.IsNullOrEmpty(this.propagationAuthUrl))
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            var encodedRedirectUrl = HttpUtility.UrlEncode(homeUrl);
            var parameters = $"?redirectUrl={encodedRedirectUrl}";

            var ssoAuthUrl = this.propagationAuthUrl.Replace("/SSO", "/LogOut") + parameters;

            return Redirect(ssoAuthUrl);
        }

        #endregion sign out

        #region re check in

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

        #endregion helpers

        #region Token

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

        private bool IsValidToken(string loginToken)
        {
            return !string.IsNullOrWhiteSpace(loginToken)
                && FormToken == loginToken
                && Request.Cookies[FormTokenName] != null
                && loginToken == Request.Cookies[FormTokenName].Value;
        }

        private void GenerateNewToken()
        {
            var loginToken = Guid.NewGuid().ToString("N");
            FormToken = loginToken;
            AddTokenToCookie(loginToken);
        }

        #endregion

        #region common

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

        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult RecheckinScript()
        {
            if (!NapSettings.Settings.EnableReCheckIn)
            {
                return Content(string.Empty, "text/javascript");
            }

            var text = Helper.ReCheckInScript;
            var rootUrl = Url.Content("~/");

            return Content($"window.rootUrl='{rootUrl}';\r\n{text}", "text/javascript");
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

        public ActionResult WhoAmI()
        {
            return Content("OK");
        }

        #region LoginFlow

        private ActionResult LoginFlow(User user)
        {
            var ignoredOtp = !NapSettings.Settings.EnableOtp || userService.IgnoredOtp(user.Id, OtpSite);

            if (ignoredOtp)
            {
                return DefaultLoginFlow(user);
            }
            else
            {
                Session["OtpUserId"] = user.Id;
                return RedirectToAction("OtpLoginFlow");
            }
        }

        private ActionResult DefaultLoginFlow(User user)
        {
            var criteria = SetUserCheckInCriteria(user.Id);
            var onUserId = securityService.Login(criteria);


            SaveUserSession(user, user.Username, onUserId);

            if (!string.IsNullOrEmpty(propagationAuthUrl))
            {
                return RedirectToPropagationAuth(user.Id, onUserId);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        [HttpGet]
        public ActionResult OtpLoginFlow()
        {
            var userId = Session["OtpUserId"] == null ? 0 : (int)Session["OtpUserId"];
            if (userId == 0)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            var ignoredOtp = !NapSettings.Settings.EnableOtp || userService.IgnoredOtp(userId, OtpSite);

            if (ignoredOtp)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult OtpLoginFlow(string otp)
        {
            ViewBag.Message = string.Empty;
            var userId = Session["OtpUserId"] == null ? 0 : (int)Session["OtpUserId"];
            var ignoredOtp = !NapSettings.Settings.EnableOtp || userService.IgnoredOtp(userId, OtpSite);
            if (userId == 0 || ignoredOtp)
            {
                Session["OtpUserId"] = 0;
                return RedirectToAction("Index");
            }

            var validOtp = !string.IsNullOrEmpty(otp) && userService.VerifyOtp(userId, OtpSite, otp);

            if (validOtp)
            {
                Session["OtpUserId"] = 0;
                var user = userService.Get(userId);
                return DefaultLoginFlow(user);
            }

            ViewBag.Message = "Invalid OTP. Please try again";

            return View();
        }

        #endregion

        #region SSO

        [HttpGet]
        public ActionResult SSO(string userId, string onuserId, string time, string redirectUrl = "")
        {
            if (ssoService.IsAuthenticated(userId, onuserId, time, this))
            {
                if (string.IsNullOrEmpty(propagationAuthUrl))
                {
                    return RedirectToRedirectUrl(redirectUrl);
                }
                else
                {
                    return RedirectToPrapagationAuth(userId, onuserId, time, redirectUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private ActionResult RedirectToRedirectUrl(string redirectUrl)
        {
            var forwardLink = string.IsNullOrEmpty(redirectUrl) ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();
            return Redirect(forwardLink);
        }


        private ActionResult RedirectToPropagationAuth(int userId, int onUserId)
        {
            var time = securityService.GetTime();
            var encodedRedirectUrl = HttpUtility.UrlEncode(homeUrl);
            var parameters = $"?userId={Cryptography.CfsEnCrypt(userId.ToString())}"
                + $"&onuserId={Cryptography.CfsEnCrypt(onUserId.ToString())}"
                + $"&time={Cryptography.CfsEnCrypt(time.ToString())}"
                + $"&redirectUrl={encodedRedirectUrl}";

            var redirectAuthUrl = propagationAuthUrl + parameters;

            return Redirect(redirectAuthUrl);
        }


        private ActionResult RedirectToPrapagationAuth(string userId, string onuserId, string time, string redirectUrl)
        {
            var absoluteRedirectUrl = Helper.GetAbsoluteUrl(redirectUrl);
            var encodedRedirectUrl = HttpUtility.UrlEncode(absoluteRedirectUrl);

            var parameters = $"?userId={userId}&onuserId={onuserId}&time={time}&redirectUrl={encodedRedirectUrl}";
            var ssoAuthUrl = this.propagationAuthUrl + parameters;
            return Redirect(ssoAuthUrl);
        }

        #endregion
    }
}
