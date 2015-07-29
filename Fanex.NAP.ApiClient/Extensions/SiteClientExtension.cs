namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class SiteClientExtension
    {
        public static SiteClient CreateSiteClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateSiteClient(source, connectionString);
        }

        public static SiteClient CreateSiteClient(this NAPClients source, string serviceUrl)
        {
            var client = new SiteClient(new Uri(serviceUrl), source.CreateDelegatingHandler());

            return client;
        }
    }
}
