namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Service.Interface;
    using Models;
    public class SiteService : ISiteService
    {
        private readonly SiteClient _siteClient;

        public SiteService()
        {
            _siteClient = ClientContext.Clients.CreateSiteClient();
        }

        public bool IsActivated(int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            return _siteClient.IsActivated(siteId);
        }

        public IEnumerable<SiteUrl> GetUrlById(int siteId, string environment)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");

            return _siteClient.GetUrlById(siteId, environment);
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(enviroment, "enviroment");

            return _siteClient.GetMenuItemBySiteId(siteId, enviroment, isActive);
        }
    }
}
