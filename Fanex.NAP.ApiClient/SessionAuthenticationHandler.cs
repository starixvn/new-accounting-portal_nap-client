using System.Web;
using System.Web.Security;
using Fanex.NAP.Core.Web.Session;

namespace Fanex.NAP.Core
{
    public static class SessionAuthenticationHandler
    {
        public static void SignOutIfSessionIsInconsistent(HttpContext context)
        {
            if (IsSessionInconsistent(context))
            {
                Signout(context);
            }
        }

        private static bool IsSessionInconsistent(HttpContext context)
        {
            return context.Request.IsAuthenticated && !IsValidSession();
        }

        private static bool IsValidSession()
        {
            var currentSession = new CurrentSession();
            return currentSession.OnUserId > 0 && currentSession.UserId > 0;
        }

        private static void Signout(HttpContext context)
        {
            FormsAuthentication.SignOut();
            var request = context.Request;
            if (request.IsAjaxRequest())
            {
                ShowAjaxUnauthorizedMessage(context);
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return (request["X-Requested-With"] == "XMLHttpRequest")
                || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        private static void ShowAjaxUnauthorizedMessage(HttpContext context)
        {
            var response = context.Response;
            response.Buffer = true;
            response.StatusCode = 403; // Unauthorized status code
            response.Write("<center><b>Unauthorized</b></center>");
            response.End();
        }

    }
}
