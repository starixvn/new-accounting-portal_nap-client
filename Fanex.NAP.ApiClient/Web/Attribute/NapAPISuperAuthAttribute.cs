namespace Fanex.NAP.Core.Web.Attribute
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Fanex.NAP.Core.Web.Session;

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NapAPISuperAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);

            ICurrentSession session = new CurrentSession();

            if (session == null || string.IsNullOrEmpty(session.Username) || session.UserId <= 0)
            {
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
        }
    }
}
