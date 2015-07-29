namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class GroupClientExtension
    {
        public static GroupClient CreateGroupClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateGroupClient(source, connectionString);
        }

        public static GroupClient CreateGroupClient(this NAPClients source, string serviceUrl)
        {
            var client = new GroupClient(new Uri(serviceUrl), source.CreateDelegatingHandler());

            return client;
        }
    }
}
