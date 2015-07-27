namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    public class MaintenanceService : BaseService, IMaintenanceService
    {
        public Downtime GetDowntimeInfo()
        {
            return Get<Downtime>(RelativePaths.GetDowntimeInfo);
        }

        private class RelativePaths
        {
            public const string GetDowntimeInfo = "/maintenance/";
        }
    }
}