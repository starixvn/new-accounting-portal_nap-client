namespace Fanex.NAP.Core.Web.Attribute
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.ApiClient.Auth;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Services;

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NapAuthAttribute : AuthorizeAttribute
    {
        private static readonly UserService UserService = new UserService();
        private static readonly Cache PermissionCache = HttpRuntime.Cache;
        private string _functionKey;
        private Constant.Policy _policy;
        private bool _outputCache = false;
        private int _duration = 0;

        public string FunctionKey
        {
            get
            {
                return this._functionKey == null ? string.Empty : this._functionKey;
            }

            set
            {
                this._functionKey = value;
            }
        }

        public Constant.Policy Policy
        {
            get
            {
                return this._policy == Constant.Policy.None ? Constant.Policy.Read : this._policy;
            }

            set
            {
                this._policy = value;
            }
        }

        public bool OutputCache
        {
            get { return _outputCache; }
            set { _outputCache = value; }
        }

        public int Duration
        {
            get { return _duration == 0 ? NapSettings.Settings.OutputCacheDuration : _duration; }
            set { _duration = value; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext == null)
            {
                this.CloseConnection(filterContext);

                return;
            }

            var principal = filterContext.HttpContext.User as NapPrincipal;

            if (principal == null)
            {
                principal = new NapPrincipal(filterContext.HttpContext);
            }

            if (object.ReferenceEquals(principal, null) || object.ReferenceEquals(principal.AuthInfo, null))
            {
                this.CloseConnection(filterContext);

                return;
            }

            if (!IsAuthenticated(principal.AuthInfo.UserId))
            {
                this.CloseConnection(filterContext);

                return;
            }
        }

        protected bool IsAuthenticated(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            int policy = 0;

            if (OutputCache)
            {
                var cacheKey = string.Format("{0}_{1}_{2}_{3}", NapSettings.Settings.AccessKey, userId, NapSettings.Settings.SiteId, this.FunctionKey);
                var cachedPolicy = PermissionCache.Get(cacheKey);

                if (cachedPolicy == null)
                {
                    policy = UserService.GetPermission(userId, NapSettings.Settings.SiteId, this.FunctionKey);
                    PermissionCache.Insert(cacheKey, policy, null, DateTime.Now.AddSeconds(Duration), Cache.NoSlidingExpiration);
                }
                else
                {
                    policy = (int)cachedPolicy;
                }
            }
            else
            {
                policy = UserService.GetPermission(userId, NapSettings.Settings.SiteId, this.FunctionKey);
            }

            return policy >= (int)this.Policy;
        }

        protected void CloseConnection(AuthorizationContext filterContext)
        {
            if (filterContext == null
                || filterContext.HttpContext == null
                || filterContext.HttpContext.Request == null)
            {
                this.SignOut();
                return;
            }

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (response == null)
            {
                this.SignOut();
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HandleForbiddenAjaxRequest(filterContext, response);
            }
            else if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                this.SignOut();
            }
            else
            {
                string settingUrl = NapSettings.Settings.UnauthorizeUrl;
                filterContext.Result = new RedirectResult(string.IsNullOrWhiteSpace(settingUrl) ? "~/Admin/Common/Unauthorize" : settingUrl);
            }
        }

        protected void HandleForbiddenAjaxRequest(AuthorizationContext filterContext, HttpResponseBase response)
        {
            var messageContent = string.Format("<center><b>{0}</b></center>", ViewResources.UnauthorizeMsg);

            filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden, messageContent);
            response.Buffer = true;
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.Write(messageContent);
            response.Flush();
            response.Close();
            response.End();
        }

        protected void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}