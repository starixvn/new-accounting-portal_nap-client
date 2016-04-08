namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;

    internal class CheckinClient : BaseClient
    {
        public CheckinClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public int ReCheckIn(int onlineUserId, int siteId)
        {
            return Get<int>(CreateRequestUri(RelativePaths.ReCheckIn, new { onUserId = onlineUserId, siteId = siteId }));
        }

        private class RelativePaths
        {
            public const string ReCheckIn = "/recheckin/";
        }
    }
}