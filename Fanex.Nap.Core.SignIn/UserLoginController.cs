using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fanex.Common;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Context;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Models.ParameterObject;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Web.Auth;
using Fanex.NAP.Core.Web.Session;

namespace Fanex.NAP.Core.SignIn
{
    public class UserLoginController : Controller
    {
        private const int TimeExipration = 30;
        private readonly ICurrentSession currentSession;
        private readonly ISecurityService securityService;
        private readonly IUserService userService;
        private readonly CurrentContext currentContext;
        public UserLoginController()
        {
            userService = new UserService();
            currentSession = new CurrentSession();
            securityService = new SecurityService();
            currentContext = new CurrentContext();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SSO(string userId, string onuserId, string time, Uri redirectUrl = null)
        {
            var currentTime = securityService.GetTime();
            var forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();
            var userIdDecrypt = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            var onuserIdDecrypt = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            var timeDecrypt = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(timeDecrypt);

            if (expiration.TotalSeconds <= TimeExipration)
            {
                var criteria = SetSsoLoginParameter(userIdDecrypt, onuserIdDecrypt);

                var onUserId = securityService.SSOLogin(criteria);
                if (onUserId != 0)
                {
                    var user = userService.Get(criteria.UserId);
                    currentSession.UserId = user.Id;
                    currentSession.Username = user.Username;
                    currentSession.OnUserId = onUserId;
                    currentSession.SessionId = Session.SessionID;
                    currentSession.Level = userService.GetUserLevel(user.Id, NapSettings.Settings.SiteId);

                    if (NapSettings.Settings.SiteId != 0)
                    {
                        currentSession.MenuItemList = userService.GetMenuItem(
                                                                                user.Id,
                                                                                NapSettings.Settings.SiteId,
                                                                                NapSettings.Settings.Environment,
                                                                                true) as IList<MenuItem>;
                    }

                    CreateAuthInfo(user);
                    return Redirect(forwardLink);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private SSOLoginParameter SetSsoLoginParameter(string userId, string onuserId)
        {
            return new SSOLoginParameter
            {
                UserId = Convert.ToInt32(userId),
                SiteId = NapSettings.Settings.SiteId,
                CheckOnUserId = Convert.ToInt32(onuserId),
                OnUserId = 0,
                IP = currentContext.IP,
                NetSessionId = Session.SessionID,
                LoginDate = DateTime.Now,
                BrowserName = currentContext.BrowserName,
                CountryCode = currentContext.CountryCode,
                Domain = currentContext.DomainName,
                Platform = currentContext.Platform
            };
        }

        private void CreateAuthInfo(User user)
        {
            var authInfo = new AuthInfo();
            authInfo.UserId = user.Id;
            authInfo.OnUserId = currentSession.OnUserId;
            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                false,
                authInfo.ToXml(),
                "/");

            var encrypted = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                HttpOnly = true,
                Path = "/",
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }
    }
}
