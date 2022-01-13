namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Business;
    using Models;
    using Models.ValueObject;

    internal class UserClient : BaseClient
    {
        public UserClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            return Get<IEnumerable<MenuItem>>(CreateRequestUri(
                string.Format(RelativePaths.GetMenuItem, userId),
                new { siteId, environment = env, isActive }));
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
            return Get<User>(CreateRequestUri(RelativePaths.GetByUsername, new { username }));
        }

        public IEnumerable<User> GetByFunctionAndSite(string functionKey, int siteId)
        {
            return Get<IEnumerable<User>>(CreateRequestUri(RelativePaths.GetByFunctionAndSite, new { functionKey, siteId }));
        }

        public int GetUserLevel(int userId, int siteId)
        {
            return Get<int>(CreateRequestUri(string.Format(RelativePaths.GetUserLevel, userId, siteId)));
        }

        public int GetPermission(int userId, int siteId, string functionKey)
        {
            return Get<int>(CreateRequestUri(string.Format(RelativePaths.GetPermission, userId), new { siteId, functionKey }));
        }

        public IEnumerable<FunctionPolicy> GetPermissions(int userId, int siteId, string parentFunctionKey, string environment = Constant.UAT)
            => Get<IEnumerable<FunctionPolicy>>(
                CreateRequestUri(string.Format(RelativePaths.GetPermissions, userId),
                new { siteId, parentFunctionKey, environment }));

        private static class RelativePaths
        {
            public const string GetById = "user/{0}";
            public const string GetByUsername = "user";
            public const string GetByFunctionAndSite = "user/byfunctionandsite";
            public const string GetPermission = "user/{0}/permission";
            public const string GetPermissions = "user/{0}/permissions";
            public const string GetMenuItem = "user/{0}/menuItem";
            public const string GetSiteLevel = "user/{0}/sitelevel";
            public const string GetUserLevel = "user/{0}/levelonsite/{1}";
        }
    }
}