namespace Fanex.NAP.Core.Web.Attribute
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NapAPIAuthAttribute : AuthorizeAttribute
    {
        private static readonly IUserService UserService = new UserService();
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private string _functionKey;
        private Constant.Policy _policy;
        private bool _permissionCache = false;
        private int _cacheDuration = 0;

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

        public bool PermissionCache
        {
            get { return _permissionCache; }
            set { _permissionCache = value; }
        }

        public int CacheDuration
        {
            get { return _cacheDuration == 0 ? NapSettings.Settings.CacheDuration : _cacheDuration; }
            set { _cacheDuration = value; }
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var session = new CurrentSession();

            if (!IsAuthenticated(session.UserId))
            {
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        protected bool IsAuthenticated(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            int policy = 0;

            if (PermissionCache)
            {
                var cacheKey = string.Format("{0}_{1}_{2}", userId, NapSettings.Settings.SiteId, this.FunctionKey);
                var cachedPolicy = Cache.Get(cacheKey);

                if (cachedPolicy == null)
                {
                    policy = UserService.GetPermission(userId, NapSettings.Settings.SiteId, this.FunctionKey);
                    Cache.Set(cacheKey, policy, DateTime.Now.AddSeconds(CacheDuration));
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
    }
}
