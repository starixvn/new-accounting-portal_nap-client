namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Web.Handler;

    internal class UserClient : BaseClient
    {
        public UserClient(Uri baseEndpoint, string appId, string secretKey)
            : base(baseEndpoint, new NAPDelegatingHandler(appId, secretKey))
        {
        }

        public UserClient(Uri baseEndpoint, DelegatingHandler handler)
            : base(baseEndpoint, handler)
        {
        }

        public bool HasAnyPermission(int userId, int siteId, string environment = Constant.UAT)
        {
            return Get<bool>(CreateRequestUri(string.Format(RelativePaths.GetByUserAndSiteIncludeGroup, userId), new { siteId = siteId, environment = environment }));
        }

        public IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            return Get<IEnumerable<MenuItem>>(CreateRequestUri(string.Format(RelativePaths.GetMenuItem, userId), new { siteId = siteId, environment = env, isActive = isActive }));
        }

        public IEnumerable<SiteLevel> GetSiteLevel(int userId, string env = Constant.UAT)
        {
            return Get<IEnumerable<SiteLevel>>(CreateRequestUri(string.Format(RelativePaths.GetSiteLevel, userId), new { environment = env }));
        }

        public User Get(int userId)
        {
            return Get<User>(CreateRequestUri(string.Format(RelativePaths.GetById, userId)));
        }

        public User Get(string username)
        {
            return Get<User>(CreateRequestUri(string.Format(RelativePaths.GetByUsername), new { username = username }));
        }

        public IEnumerable<User> GetAll()
        {
            return Get<IEnumerable<User>>(CreateRequestUri(RelativePaths.GetAll));
        }

        public int GetUserLevel(int userId, int siteId)
        {
            return Get<int>(CreateRequestUri(string.Format(RelativePaths.GetUserLevel, userId, siteId)));
        }

        public int GetPermission(int userId, int siteId, string functionKey)
        {
            return Get<int>(CreateRequestUri(string.Format(RelativePaths.GetPermission, userId), new { siteId = siteId, functionKey = functionKey }));
        }

        private class RelativePaths
        {
            public const string GetByUserAndSiteIncludeGroup = "/user/{0}/functionPolicy";
            public const string GetMenuItem = "/user/{0}/menuItem";
            public const string GetSiteLevel = "/user/{0}/sitelevel";
            public const string GetById = "/user/{0}";
            public const string GetByUsername = "/user";
            public const string GetAll = "/user/";
            public const string GetUserLevel = "/user/{0}/levelonsite/{1}";
            public const string GetPermission = "/user/{0}/sitepolicy";
        }
    }
}
