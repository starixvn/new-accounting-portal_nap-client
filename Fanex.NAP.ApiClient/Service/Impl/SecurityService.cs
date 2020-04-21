namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;

    public class SecurityService : ISecurityService
    {
        public int Login(LoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");
            using (var loginClient = ClientContext.Clients.CreateLoginClient())
            {
                return loginClient.Login(parameter);
            }
        }

        public void Logout(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");
            using (var logoutClient = ClientContext.Clients.CreateLogoutClient())
            {
                logoutClient.Logout(onlineUserId);
            }
        }

        public int ReCheckIn(int onlineUserId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            using (var checkinClient = ClientContext.Clients.CreateReCheckinClient())
            {
                return checkinClient.ReCheckIn(onlineUserId, siteId);
            }
        }

        public string GetTime()
        {
            using (var ssoClient = ClientContext.Clients.CreateSSOClient())
            {
                return ssoClient.GetTime();
            }
        }

        public int SSOLogin(SSOLoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");
            using (var ssoClient = ClientContext.Clients.CreateSSOClient())
            {
                return ssoClient.Login(parameter);
            }
        }

        public bool UpdatePassword(int siteId, int userId, string password)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNegativeOrZero(userId, "parameter");
            Check.Argument.IsNotEmpty(password, "password");
            using (var userInfoClient = ClientContext.Clients.CreateUserInfoClient())
            {
                return userInfoClient.UpdatePassword(siteId, userId, password);
            }
        }

        public bool UpdateProfile(int siteId, UpdateUserInfoParameter parameter)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNull(parameter, "parameter");
            using (var userInfoClient = ClientContext.Clients.CreateUserInfoClient())
            {
                return userInfoClient.UpdateProfile(siteId, parameter);
            }
        }
    }
}