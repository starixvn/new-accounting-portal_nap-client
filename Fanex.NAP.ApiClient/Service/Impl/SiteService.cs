namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Service.Interface;
    using Models;

    public class SiteService : ISiteService
    {
        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(enviroment, "enviroment");
            var siteClient = ClientContext.Clients.CreateSiteClient();
            return siteClient.GetMenuItemBySiteId(siteId, enviroment, isActive);
        }
    }
}