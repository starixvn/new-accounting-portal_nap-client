namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class LoginClientExtension
    {
        public static LoginClient CreateLoginClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateLoginClient(source, connectionString);
        }

        public static LoginClient CreateLoginClient(this NAPClients source, string serviceUrl)
        {
            var client = new LoginClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}
