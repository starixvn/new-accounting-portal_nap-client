namespace Fanex.NAP.Core.Service.Impl
{
    using Clients;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Service.Interface;
    using Models.ParameterObject;

    public class SecurityService : ISecurityService
    {
        public int Login(LoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");
            var loginClient = ClientContext.Clients.CreateLoginClient();

            return loginClient.Login(parameter);
        }

        public void Logout(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");
            var logoutClient = ClientContext.Clients.CreateLogoutClient();
            logoutClient.Logout(onlineUserId);
        }

        public int ReCheckIn(int onlineUserId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            var checkinClient = ClientContext.Clients.CreateReCheckinClient();

            return checkinClient.ReCheckIn(onlineUserId, siteId);

        }

        public string GetTime()
        {
            var ssoClient = ClientContext.Clients.CreateSSOClient();

            return ssoClient.GetTime();
        }

        public int SSOLogin(SSOLoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");
            var ssoClient = ClientContext.Clients.CreateSSOClient();
            return ssoClient.Login(parameter);
        }

        public bool UpdatePassword(int siteId, int userId, string password)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNegativeOrZero(userId, "parameter");
            Check.Argument.IsNotEmpty(password, "password");
            var userInfoClient = ClientContext.Clients.CreateUserInfoClient();
            return userInfoClient.UpdatePassword(siteId, userId, password);
        }
    }
}