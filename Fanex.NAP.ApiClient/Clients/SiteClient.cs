namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Models;

    internal class SiteClient : BaseClient
    {
        public SiteClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string env, bool? isActive = null)
        {
            return Get<IEnumerable<MenuItem>>(CreateRequestUri(string.Format(RelativePaths.GetMenuItemBySiteId, siteId), new { environment = env, isActive = isActive }));
        }

        private class RelativePaths
        {
            public const string GetMenuItemBySiteId = "site/{0}/menuItem";
        }
    }
}