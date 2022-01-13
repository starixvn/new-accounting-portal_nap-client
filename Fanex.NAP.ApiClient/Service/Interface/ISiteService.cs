namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Models;

    public interface ISiteService
    {
        IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string enviroment, bool? isActive = null);
    }
}