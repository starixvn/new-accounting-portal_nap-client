namespace Fanex.NAP.Core.Extensions
{
    using System;
    using Fanex.NAP.Core.Clients;

    internal static class MaintenanceClientExtension
    {
        public static MaintenanceClient CreateMaintenanceClient(this NAPClients source)
        {
            var connectionString = ClientContext.Configuration.ServiceUrl;
            return CreateMaintenanceClient(source, connectionString);
        }

        public static MaintenanceClient CreateMaintenanceClient(this NAPClients source, string serviceUrl)
        {
            var client = new MaintenanceClient(new Uri(serviceUrl), source.CreateDelegatingHandler());

            return client;
        }
    }
}
