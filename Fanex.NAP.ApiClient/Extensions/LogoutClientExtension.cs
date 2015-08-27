namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class LogoutClientExtension
    {
        public static LogoutClient CreateLogoutClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateLogoutClient(source, connectionString);
        }

        public static LogoutClient CreateLogoutClient(this NAPClients source, string serviceUrl)
        {
            var client = new LogoutClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}
