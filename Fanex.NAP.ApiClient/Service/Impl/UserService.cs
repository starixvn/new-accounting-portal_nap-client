namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Security.Permissions;
    using Business;
    using Clients;
    using Common.Helper;
    using Extensions;
    using Interface;
    using Models;
    using Models.ValueObject;

    public class UserService : IUserService
    {
        public IEnumerable<MenuItem> GetMenuItem(int userId, int siteId, string environment = Constant.UAT, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(environment, "environment");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.GetMenuItem(userId, siteId, environment, isActive);

        }

        public IEnumerable<SiteLevel> GetSiteLevel(int userId, string environment = Constant.UAT)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotEmpty(environment, "environment");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.GetSiteLevel(userId, environment);

        }

        public User Get(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.Get(userId);
        }

        public User Get(string username)
        {
            Check.Argument.IsNotEmpty(username, "username");
            var userClient = ClientContext.Clients.CreateUserClient();
            {
                return userClient.Get(username);
            }
        }

        public IEnumerable<User> GetBySite(int siteId, bool? isActive)
        {
            var userClient = ClientContext.Clients.CreateUserClient();
            {
                return userClient.GetBySite(siteId, isActive);
            }
        }

        public IEnumerable<User> GetByFunctionAndSite(string functionKey, int siteId)
        {
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.GetByFunctionAndSite(functionKey, siteId);
        }

        public int GetUserLevel(int userId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            var userClient = ClientContext.Clients.CreateUserClient();
            return userClient.GetUserLevel(userId, siteId);
        }

        public int GetPermission(int userId, int siteId, string functionKey)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(functionKey, "functionKey");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.GetPermission(userId, siteId, functionKey);
        }

        public bool IsHavingPermission(int userId, int siteId, string functionKey, Constant.Policy policy)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(functionKey, "functionKey");

            int currentPolicy = GetPermission(userId, siteId, functionKey);

            return currentPolicy >= (int)policy;
        }

        public IEnumerable<FunctionPolicy> GetPermissions(int userId, int siteId, string parentFunctionKey, string environment = Constant.UAT)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.GetPermissions(userId, siteId, parentFunctionKey, environment);
        }

        public bool IgnoredOtp(int userId, string system)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.IgnoredOtp(userId, system);
        }

        public bool VerifyOtp(int userId, string system, string otp)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            var userClient = ClientContext.Clients.CreateUserClient();

            return userClient.VerifyOtp(userId, system, otp);
        }
    }
}