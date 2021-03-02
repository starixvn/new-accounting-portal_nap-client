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
        public bool IsActivated(int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            var siteClient = ClientContext.Clients.CreateSiteClient();
            return siteClient.IsActivated(siteId);
        }

        public IEnumerable<SiteUrl> GetUrlById(int siteId, string environment)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");
            var siteClient = ClientContext.Clients.CreateSiteClient();
            return siteClient.GetUrlById(siteId, environment);
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(enviroment, "enviroment");
            var siteClient = ClientContext.Clients.CreateSiteClient();
            return siteClient.GetMenuItemBySiteId(siteId, enviroment, isActive);
        }
    }
}