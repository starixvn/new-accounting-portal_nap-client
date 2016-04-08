namespace Fanex.NAP.Core.Extensions
{
    using Fanex.NAP.Core.Clients;
    using System;

    internal static class SiteClientExtension
    {
        public static SiteClient CreateSiteClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateSiteClient(source, connectionString);
        }

        public static SiteClient CreateSiteClient(this NAPClients source, string serviceUrl)
        {
            var client = new SiteClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}