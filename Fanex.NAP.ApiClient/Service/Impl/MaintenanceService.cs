namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    public class MaintenanceService : IMaintenanceService
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
