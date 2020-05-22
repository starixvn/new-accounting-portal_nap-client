namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common;
    using Configuration;
    using Context;
    using Interface;
    using Models;
    using Models.ParameterObject;
    using Web.Auth;
    using Web.Session;

    public class SSOServiceImp : SSOService
    {
        private const int TimeExipration = 30;
        private readonly ISecurityService securityService;
        private readonly ICurrentSession currentSession;
        private readonly IUserService userService;
        private readonly CurrentContext currentContext;

        public SSOServiceImp()
        {
            userService = new UserService();
            currentSession = new CurrentSession();
            securityService = new SecurityService();
            currentContext = new CurrentContext();
        }

        public bool IsAuthenticated(string userId, string onuserId, string time, Controller controller)
        {
            var currentTime = securityService.GetTime();
            var userIdDecrypt = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            var onuserIdDecrypt = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            var timeDecrypt = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(timeDecrypt);

            if (expiration.TotalSeconds <= TimeExipration)
            {
                var criteria = SetSsoLoginParameter(userIdDecrypt, onuserIdDecrypt, controller.Session.SessionID);

                var onUserId = securityService.SSOLogin(criteria);
                if (onUserId != 0)
                {
                    var user = userService.Get(criteria.UserId);
                    currentSession.UserId = user.Id;
                    currentSession.Username = user.Username;
                    currentSession.OnUserId = onUserId;
                    currentSession.SessionId = controller.Session.SessionID;
                    currentSession.Level = userService.GetUserLevel(user.Id, NapSettings.Settings.SiteId);

                    if (NapSettings.Settings.SiteId != 0)
                    {
                        currentSession.MenuItemList = userService.GetMenuItem(
                                                                                user.Id,
                                                                                NapSettings.Settings.SiteId,
                                                                                NapSettings.Settings.Environment,
                                                                                true) as IList<MenuItem>;
                    }

                    CreateAuthInfo(user, controller.Response);
                    return true;
                }
            }

            return false;
        }

        private SSOLoginParameter SetSsoLoginParameter(string userId, string onuserId, string netSessionId)
        {
            return new SSOLoginParameter
            {
                UserId = Convert.ToInt32(userId),
                SiteId = NapSettings.Settings.SiteId,
                CheckOnUserId = Convert.ToInt32(onuserId),
                OnUserId = 0,
                IP = currentContext.IP,
                NetSessionId = netSessionId,
                LoginDate = DateTime.Now,
                BrowserName = currentContext.BrowserName,
                CountryCode = currentContext.CountryCode,
                Domain = currentContext.DomainName,
                Platform = currentContext.Platform
            };
        }

        private void CreateAuthInfo(User user, HttpResponseBase response)
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

            response.Cookies.Add(authCookie);
        }
    }
}