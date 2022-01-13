namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Business;
    using Models;
    using Models.ValueObject;

    public interface IUserService
    {
        User Get(int userId);

        User Get(string username);

        int GetUserLevel(int userId, int siteId);

        IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string environment = Constant.UAT, bool? isActive = null);

        int GetPermission(int userId, int siteId, string functionKey);

        IEnumerable<SiteLevel> GetSiteLevel(int userId, string environment = Constant.UAT);

        bool IsHavingPermission(int userId, int siteId, string functionKey, Constant.Policy policy);

        IEnumerable<FunctionPolicy> GetPermissions(int userId, int siteId, string parentFunctionKey, string environment = Constant.UAT);
    }
}