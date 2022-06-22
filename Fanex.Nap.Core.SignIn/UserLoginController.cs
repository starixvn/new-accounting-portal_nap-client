namespace Fanex.NAP.Core.SignIn
{
    using System.Web.Mvc;

    public class UserLoginController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SSO(string userId, string onuserId, string time, string redirectUrl = "")
        {
            return RedirectToAction("SSO", "Auth", new { userId, onuserId, time, redirectUrl });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return RedirectToAction("Index", "Auth");
        }
    }
}