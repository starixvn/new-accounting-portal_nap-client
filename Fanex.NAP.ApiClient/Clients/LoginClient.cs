namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Models.ParameterObject;

    internal class LoginClient : BaseClient
    {
        public LoginClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public int Login(LoginParameter parameter)
        {
            return Post<LoginParameter, int>(CreateRequestUri(RelativePaths.Login), parameter);
        }

        private class RelativePaths
        {
            public const string Login = "login";
        }
    }
}