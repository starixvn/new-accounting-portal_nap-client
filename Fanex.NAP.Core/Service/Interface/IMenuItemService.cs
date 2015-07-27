namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Domain;

    public interface IMenuItemService : IBaseService
    {
        IEnumerable<IMenuItem> GetByUserAndSiteIncludeGroup(
            int userId,
            int siteId,
            string env = Constant.Uat,
            bool? isActive = null);
    }
}