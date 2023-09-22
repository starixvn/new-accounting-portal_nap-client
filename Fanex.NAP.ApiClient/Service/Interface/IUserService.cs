namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Business;
using Fanex.Common.Helper;
using Fanex.NAP.Core.Clients;
    using Models;
    using Models.ValueObject;

    public interface IUserService
    {
        User Get(int userId);

        User Get(string username);

        IEnumerable<User> GetBySite(int siteId, bool? isActive);

        int GetUserLevel(int userId, int siteId);

        IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string environment = Constant.UAT, bool? isActive = null);

        int GetPermission(int userId, int siteId, string functionKey);

        IEnumerable<SiteLevel> GetSiteLevel(int userId, string environment = Constant.UAT);

        bool IsHavingPermission(int userId, int siteId, string functionKey, Constant.Policy policy);

        IEnumerable<FunctionPolicy> GetPermissions(int userId, int siteId, string parentFunctionKey, string environment = Constant.UAT);

        bool IgnoredOtp(int userId, string system);

        bool VerifyOtp(int userId, string system, string otp);
    }
}