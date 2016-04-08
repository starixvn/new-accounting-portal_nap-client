namespace Fanex.NAP.Core.Extensions
{
    using Fanex.NAP.Core.Clients;
    using System;

    internal static class LogoutClientExtension
    {
        public static LogoutClient CreateLogoutClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateLogoutClient(source, connectionString);
        }

        public static LogoutClient CreateLogoutClient(this NAPClients source, string serviceUrl)
        {
            var client = new LogoutClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}