namespace Fanex.Nap.SignIn.Attribute
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Super authorize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Authorize action handler.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            bool skipAuthorization = SkipAuthorization(filterContext);

            if (skipAuthorization)
            {
                return;
            }

            if (!AuthorizeCore(filterContext.HttpContext) || SessionIsValid())
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpUnauthorizedResult("<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");
            }
            else
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
            }
        }

        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
        }

        protected bool SessionIsValid()
        {
            var session = new CurrentSession();
            return !string.IsNullOrEmpty(session.Username) && session.UserId > 0;
        }
    }
}