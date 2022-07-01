namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Clients;

    internal static class UserInfoClientExtension
    {
        public static UserInfoClient CreateUserInfoClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateUserInfoClient(source, connectionString);
        }

        public static UserInfoClient CreateUserInfoClient(this NAPClients source, string serviceUrl)
        {
            var client = new UserInfoClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}