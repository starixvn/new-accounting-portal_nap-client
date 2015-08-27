namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class UserClientExtension
    {
        public static UserClient CreateUserClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateUserClient(source, connectionString);
        }

        public static UserClient CreateUserClient(this NAPClients source, string serviceUrl)
        {
            var client = new UserClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}
