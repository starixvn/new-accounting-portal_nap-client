namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Web.Handler;
    using Models;
    internal class SiteClient : BaseClient
    {
        public SiteClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public bool IsActivated(int siteId)
        {
            return Get<bool>(CreateRequestUri(string.Format(RelativePaths.IsActivated, siteId)));
        }

        public IEnumerable<SiteUrl> GetUrlById(int siteId, string env)
        {
            return Get<IEnumerable<SiteUrl>>(CreateRequestUri(string.Format(RelativePaths.GetUrlById, siteId), new { environment = env }));
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string env, bool? isActive = null)
        {
            return Get<IEnumerable<MenuItem>>(CreateRequestUri(string.Format(RelativePaths.GetMenuItemBySiteId, siteId), new { environment = env, isActive = isActive}));
        }

        private class RelativePaths
        {
            public const string IsActivated = "/site/{0}/status";
            public const string GetUrlById = "/site/{0}/siteUrl";
            public const string GetMenuItemBySiteId = "/site/{0}/menuItem";
        }
    }
}
