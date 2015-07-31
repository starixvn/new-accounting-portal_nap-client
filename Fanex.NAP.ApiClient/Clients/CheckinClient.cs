namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Web.Handler;

    internal class CheckinClient : BaseClient
    {
        public CheckinClient(Uri baseEndpoint, DelegatingHandler handler)
            : base(baseEndpoint, handler)
        {
        }

        public bool ReCheckIn(int onlineUserId)
        {
            return Send<int, bool>(CreateRequestUri(RelativePaths.ReCheckIn), HttpMethod.Post, onlineUserId);
        }

        private class RelativePaths
        {
            public const string ReCheckIn = "/recheckin/";
        }
    }
}
