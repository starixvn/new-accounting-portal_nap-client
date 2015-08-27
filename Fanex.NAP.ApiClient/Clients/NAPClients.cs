namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Web.Handler;

    internal sealed class NAPClients
    {
        internal NAPClients()
        {
        }

        public Func<HttpClient> CreateHttpClient { get; set; }
    }
}
