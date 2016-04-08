namespace Fanex.NAP.Core.Extensions
{
    using Fanex.NAP.Core.Clients;
    using System;

    internal static class CheckinClientExtension
    {
        public static CheckinClient CreateReCheckinClient(this NAPClients source)
        {
            var connectionString = ClientContext.GetServiceUrl();
            return CreateReCheckinClient(source, connectionString);
        }

        public static CheckinClient CreateReCheckinClient(this NAPClients source, string serviceUrl)
        {
            var client = new CheckinClient(new Uri(serviceUrl), source.CreateHttpClient());

            return client;
        }
    }
}