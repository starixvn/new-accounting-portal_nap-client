namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using Fanex.NAP.Core.Models.ValueObject;

    public interface IMaintenanceService
    {
        Downtime GetDowntime();
    }
}
