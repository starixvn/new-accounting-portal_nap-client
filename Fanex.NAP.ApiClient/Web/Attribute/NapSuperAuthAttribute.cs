namespace Fanex.NAP.Core.Web.Attribute
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Super authorize attribute.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NapSuperAuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Authorize action handler.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext == null)
            {
                return;
            }

            if (!CheckSessionAuthorization())
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpUnauthorizedResult("<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");
                }
                else
                {
                    filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                }
            }
        }

        protected bool CheckSessionAuthorization()
        {
            ICurrentSession session = new CurrentSession();

            if (session == null || string.IsNullOrEmpty(session.Username) || session.UserId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}