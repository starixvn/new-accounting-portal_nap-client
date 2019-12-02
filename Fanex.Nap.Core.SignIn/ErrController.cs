using System.Web.Mvc;

namespace Fanex.NAP.Core.SignIn
{
    public class ErrController : Controller
    {
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View("Fanex.NAP.Core.SignIn.Views.NotFound");
        }
    }
}
