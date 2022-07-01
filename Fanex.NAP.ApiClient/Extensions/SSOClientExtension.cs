namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Clients;

    internal static class SSOClientExtension
    {
        public static SSOClient CreateSSOClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateGroupClient(source, connectionString);
        }

        public static SSOClient CreateGroupClient(this NAPClients source, string serviceUrl)
        {
            var client = new SSOClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}