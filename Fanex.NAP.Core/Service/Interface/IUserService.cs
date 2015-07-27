namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ParameterObject;

    public interface IUserService : IBaseService
    {
        IUser GetById(int id);

        IUser GetByUsername(string username);

        IEnumerable<IUser> GetAll();

        int GetUserSiteLevel(int userId, int siteId);

        int UserCheckIn(UserCheckInUser parameter);

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        void UserLogOut(int onUserId);

        void UserSSOCheckIn(UserSSOCheckInUser parameter);

        string UserSSOGetTime();

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        bool ReUserCheckIn(int onUserId);

        int GetUserSiteFunctionKeyPolicy(int userId, int siteId, string functionKey);

        bool HasPermissionUserSiteFunctionKey(int userId, int siteId, string functionKey, Constant.Policy policy);
    }
}
