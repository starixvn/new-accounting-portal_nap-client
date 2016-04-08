namespace Fanex.NAP.Core.Extensions
{
    using Fanex.NAP.Core.Clients;
    using System;

    internal static class GroupClientExtension
    {
        public static GroupClient CreateGroupClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateGroupClient(source, connectionString);
        }

        public static GroupClient CreateGroupClient(this NAPClients source, string serviceUrl)
        {
            var client = new GroupClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}