namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using Fanex.NAP.Core.Models.ParameterObject;

    public interface ISecurityService
    {
        string GetTime();

        int Login(LoginParameter parameter);

        void Logout(int onlineUserId);

        bool ReCheckIn(int onlineUserId);

        int SSOLogin(SSOLoginParameter parameter);
    }
}
