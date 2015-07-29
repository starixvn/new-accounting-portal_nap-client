namespace Fanex.NAP.Core.Services
{
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ValueObject;

    public class MaintenanceService
    {
        private readonly MaintenanceClient _maintenanceClient;

        public MaintenanceService()
        {
            _maintenanceClient = ClientContext.Clients.CreateMaintenanceClient();
        }

        public Downtime GetDowntime()
        {
            return _maintenanceClient.GetDowntime();
        }
    }
}
