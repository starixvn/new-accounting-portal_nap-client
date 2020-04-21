namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Service.Interface;
    using Models;
    using System.Collections.Generic;

    public class SiteService : ISiteService
    {
        public bool IsActivated(int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            using (var siteClient = ClientContext.Clients.CreateSiteClient())
            {
                return siteClient.IsActivated(siteId);
            }
        }

        public IEnumerable<SiteUrl> GetUrlById(int siteId, string environment)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");
            using (var siteClient = ClientContext.Clients.CreateSiteClient())
            {
                return siteClient.GetUrlById(siteId, environment);
            }
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(enviroment, "enviroment");
            using (var siteClient = ClientContext.Clients.CreateSiteClient())
            {
                return siteClient.GetMenuItemBySiteId(siteId, enviroment, isActive);
            }
        }
    }
}