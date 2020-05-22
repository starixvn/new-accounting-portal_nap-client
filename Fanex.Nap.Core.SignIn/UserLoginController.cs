namespace Fanex.NAP.Core.SignIn
{
    using System;
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
        public ActionResult SSO(string userId, string onuserId, string time, Uri redirectUrl = null)
        {
            if (ssoService.IsAuthenticated(userId, onuserId, time, this))
            {
                var forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();

                return Redirect(forwardLink);
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}