namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Models.ValueObject;
    using Models;
    public interface ISiteService
    {
        IEnumerable<SiteUrl> GetUrlById(int siteId, string environment);

        bool IsActivated(int siteId);

        IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null);
    }
}
