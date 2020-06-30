namespace Fanex.NAP.Core.SignIn
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Service.Impl;
    using Service.Interface;

    public class UserLoginController : Controller
    {
        private readonly SSOService ssoService;

        public UserLoginController()
        {
            ssoService = new SSOServiceImp();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SSO(string userId, string onuserId, string time, string redirectUrl = "")
        {
            if (ssoService.IsAuthenticated(userId, onuserId, time, this))
            {
                var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
                if (!string.IsNullOrEmpty(aspNetCoreAuth))
                {
                    return RedirectToAspNetCore(userId, onuserId, time, redirectUrl);
                }
                else
                {
                    return RedirectToRedirectUrl(redirectUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private ActionResult RedirectToRedirectUrl(string redirectUrl)
        {
            var forwardLink = string.IsNullOrEmpty(redirectUrl) ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();
            return Redirect(forwardLink);
        }

        private ActionResult RedirectToAspNetCore(string userId, string onuserId, string time, string redirectUrl)
        {
            redirectUrl = string.IsNullOrEmpty(redirectUrl)
                                ? new Uri(Request.Url, FormsAuthentication.DefaultUrl).ToString()
                                : redirectUrl;
            redirectUrl = HttpUtility.UrlEncode(redirectUrl);

            var applicationPath = Request.ApplicationPath.Replace("/", "");
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"].Replace("{ApplicationPath}", applicationPath);
            var aspNetCoreSsoUrl = aspNetCoreAuth
                + $"/SSO?userId={userId}&onuserId={onuserId}&time={time}&redirectUrl={redirectUrl}";
            return Redirect(aspNetCoreSsoUrl);
        }
    }
}