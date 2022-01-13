namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Clients;

    internal static class UserClientExtension
    {
        public static UserClient CreateUserClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateUserClient(source, connectionString);
        }

        public static UserClient CreateUserClient(this NAPClients source, string serviceUrl)
        {
            var client = new UserClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}