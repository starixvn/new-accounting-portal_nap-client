namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Web.Handler;

    internal class MaintenanceClient : BaseClient
    {
        public MaintenanceClient(Uri baseEndpoint, DelegatingHandler handler)
            : base(baseEndpoint, handler)
        {
        }

        public Downtime GetDowntime()
        {
            return Get<Downtime>(CreateRequestUri(RelativePaths.GetDowntime));
        }

        private class RelativePaths
        {
            public const string GetDowntime = "/maintenance/";
        }
    }
}
