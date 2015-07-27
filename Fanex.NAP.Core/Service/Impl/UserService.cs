namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;

    public class UserService : BaseService, IUserService
    {
        public IUser GetById(int id)
        {
            return Get<User>(string.Format(RelativePaths.GetById, id));
        }

        public IUser GetByUsername(string username)
        {
            return Get<User>(string.Format(RelativePaths.GetByUsername, username));
        }

        public IEnumerable<IUser> GetAll()
        {
            return Get<IEnumerable<User>>(RelativePaths.GetAll);
        }

        public int GetUserSiteLevel(int userId, int siteId)
        {
            return Get<int>(string.Format(RelativePaths.GetUserSiteLevel, userId, siteId));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public void UserLogOut(int onUserId)
        {
            Post<string, int>(onUserId, RelativePaths.UserLogOut);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public string UserSSOGetTime()
        {
            return Get<string>(RelativePaths.UserSSOGetTime);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public bool ReUserCheckIn(int onUserId)
        {
            return Post<bool, int>(onUserId, RelativePaths.ReUserCheckIn);
        }

        public int GetUserSiteFunctionKeyPolicy(int userId, int siteId, string functionKey)
        {
            return Get<int>(string.Format(RelativePaths.GetUserSiteFunctionKeyPolicy, userId, siteId, functionKey));
        }

        public int UserCheckIn(UserCheckInUser parameter)
        {
            return Post<int, UserCheckInUser>(parameter, RelativePaths.UserCheckIn);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public void UserSSOCheckIn(UserSSOCheckInUser parameter)
        {
            var onUserId = Post<int, UserSSOCheckInUser>(parameter, RelativePaths.UserSSOCheckIn);

            parameter.OnUserId = onUserId;
        }

        public bool HasPermissionUserSiteFunctionKey(int userId, int siteId, string functionKey, Constant.Policy policy)
        {
            int tempPolicy = this.GetUserSiteFunctionKeyPolicy(userId, siteId, functionKey);

            if (tempPolicy >= (int)policy)
            {
                return true;
            }

            return false;
        }

        private class RelativePaths
        {
            public const string GetById = "/user/{0}";
            public const string GetByUsername = "/user?username={0}";
            public const string GetAll = "/user/";
            public const string GetUserSiteLevel = "/user/{0}/levelonsite/{1}";
            public const string GetUserSiteFunctionKeyPolicy = "/user/{0}/sitepolicy?siteId={1}&functionKey={2}";
            public const string UserLogOut = "/logout/";
            public const string UserCheckIn = "/login/";
            public const string ReUserCheckIn = "/recheckin/";
            public const string UserSSOGetTime = "/ssologin/";
            public const string UserSSOCheckIn = "/ssologin/";
        }
    }
}