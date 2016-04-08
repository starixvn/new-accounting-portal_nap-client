namespace Fanex.NAP.Core.Service.Interface
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ValueObject;
    using Models.ParameterObject;
    using System.Collections.Generic;

    public interface IUserService
    {
        User Get(int userId);

        User Get(string username);

        IEnumerable<User> GetAll();

        IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string environment = Constant.UAT, bool? isActive = null);

        int GetPermission(int userId, int siteId, string functionKey);

        IEnumerable<SiteLevel> GetSiteLevel(int userId, string environment = Constant.UAT);

        int GetUserLevel(int userId, int siteId);

        bool HasAnyPermission(int userId, int siteId, string environment = Constant.UAT);

        bool IsHavingPermission(int userId, int siteId, string functionKey, Constant.Policy policy);

        bool UpdateProfile(UpdateUserInfoParameter parameter);
    }
}