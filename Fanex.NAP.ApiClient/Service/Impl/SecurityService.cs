namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;

    public class SecurityService : ISecurityService
    {
        private readonly LoginClient _loginClient;
        private readonly LogoutClient _logoutClient;
        private readonly CheckinClient _checkinClient;
        private readonly SSOClient _ssoClient;
        private readonly UserInfoClient _userInfoClient;

        public SecurityService()
        {
            _loginClient = ClientContext.Clients.CreateLoginClient();
            _logoutClient = ClientContext.Clients.CreateLogoutClient();
            _checkinClient = ClientContext.Clients.CreateReCheckinClient();
            _ssoClient = ClientContext.Clients.CreateSSOClient();
            _userInfoClient = ClientContext.Clients.CreateUserInfoClient();
        }

        public int Login(LoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");

            return _loginClient.Login(parameter);
        }

        public void Logout(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");

            _logoutClient.Logout(onlineUserId);
        }

        public int ReCheckIn(int onlineUserId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            return _checkinClient.ReCheckIn(onlineUserId, siteId);
        }

        public string GetTime()
        {
            return _ssoClient.GetTime();
        }

        public int SSOLogin(SSOLoginParameter parameter)
        {
            Check.Argument.IsNotNull(parameter, "parameter");

            return _ssoClient.Login(parameter);
        }

        public bool UpdatePassword(int siteId, int userId, string password)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNegativeOrZero(userId, "parameter");
            Check.Argument.IsNotEmpty(password, "password");

            return _userInfoClient.UpdatePassword(siteId, userId, password);
        }

        public bool UpdateProfile(int siteId, UpdateUserInfoParameter parameter)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNull(parameter, "parameter");

            return _userInfoClient.UpdateProfile(siteId, parameter);
        }
    }
}