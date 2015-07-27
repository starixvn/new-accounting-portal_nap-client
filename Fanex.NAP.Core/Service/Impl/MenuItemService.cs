namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Interface;

    public class MenuItemService : BaseService, IMenuItemService
    {
        public IEnumerable<IMenuItem> GetByUserAndSiteIncludeGroup(int userId, int siteId, string env = Constant.Uat, bool? isActive = null)
        {
            return Get<IEnumerable<MenuItem>>(string.Format(RelativePaths.GetByUserAndSiteIncludeGroup, userId, siteId, env, isActive));
        }

        private class RelativePaths
        {
            public const string GetByUserAndSiteIncludeGroup = "/user/{0}/menuItem?siteId={1}&environment={2}&isActive={3}";
        }
    }
}