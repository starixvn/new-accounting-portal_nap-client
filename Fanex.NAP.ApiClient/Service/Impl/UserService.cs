namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    public class UserService : IUserService
    {
        private readonly UserClient _userClient;

        public UserService()
        {
            _userClient = ClientContext.Clients.CreateUserClient();
        }

        public bool HasAnyPermission(int userId, int siteId, string environment = Constant.UAT)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");

            return _userClient.HasAnyPermission(userId, siteId, environment);
        }

        public IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string environment = Constant.UAT, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");

            return _userClient.GetMenuItem(userId, siteId, environment, isActive);
        }

        public IEnumerable<SiteLevel> GetSiteLevel(int userId, string environment = Constant.UAT)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotEmpty(environment, "environment");

            return _userClient.GetSiteLevel(userId, environment);
        }

        public User Get(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");

            return _userClient.Get(userId);
        }

        public User Get(string username)
        {
            Check.Argument.IsNotEmpty(username, "username");

            return _userClient.Get(username);
        }

        public IEnumerable<User> GetAll()
        {
            return _userClient.GetAll();
        }

        public int GetUserLevel(int userId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            return _userClient.GetUserLevel(userId, siteId);
        }

        public int GetPermission(int userId, int siteId, string functionKey)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(functionKey, "functionKey");

            return _userClient.GetPermission(userId, siteId, functionKey);
        }

        public bool IsHavingPermission(int userId, int siteId, string functionKey, Constant.Policy policy)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(functionKey, "functionKey");

            int currentPolicy = GetPermission(userId, siteId, functionKey);

            return currentPolicy >= (int)policy;
        }
    }
}
