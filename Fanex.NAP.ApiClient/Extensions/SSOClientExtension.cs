namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class SSOClientExtension
    {
        public static SSOClient CreateSSOClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateGroupClient(source, connectionString);
        }

        public static SSOClient CreateGroupClient(this NAPClients source, string serviceUrl)
        {
            var client = new SSOClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}
