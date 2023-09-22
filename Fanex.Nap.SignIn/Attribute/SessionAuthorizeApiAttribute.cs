namespace Fanex.Nap.SignIn.Attribute
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Fanex.NAP.Core.Web.Session;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionAuthorizeApiAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (SkipAuthorization(actionContext))
            {
                return;
            }

            if (SessionIsDisabled() || !IsAuthorized(actionContext))
            {
                return;
            }

            if (!SessionIsValid())
            {
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private static bool SessionIsValid()
        {
            var session = new CurrentSession();
            return !string.IsNullOrEmpty(session.Username) && session.UserId > 0;
        }

        private static bool SessionIsDisabled()
        {
            return HttpContext.Current.Session == null;
        }

    }
}
