namespace Fanex.NAP.Core.Services
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models.ParameterObject;

    public class SecurityService
    {
        private readonly LoginClient _loginClient;
        private readonly LogoutClient _logoutClient;
        private readonly CheckinClient _checkinClient;
        private readonly SSOClient _ssoClient;

        public SecurityService()
        {
            _loginClient = ClientContext.Clients.CreateLoginClient();
            _logoutClient = ClientContext.Clients.CreateLogoutClient();
            _checkinClient = ClientContext.Clients.CreateReCheckinClient();
            _ssoClient = ClientContext.Clients.CreateSSOClient();
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

        public bool ReCheckIn(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onlineUserId");

            return _checkinClient.ReCheckIn(onlineUserId);
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
    }
}
