namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Web.Handler;

    internal class LoginClient : BaseClient
    {
        public LoginClient(Uri baseEndpoint, string appId, string secretKey)
            : base(baseEndpoint, new NAPDelegatingHandler(appId, secretKey))
        {
        }

        public LoginClient(Uri baseEndpoint, DelegatingHandler handler)
            : base(baseEndpoint, handler)
        {
        }

        public int Login(LoginParameter parameter)
        {
            return Send<LoginParameter, int>(CreateRequestUri(RelativePaths.Login), HttpMethod.Post, parameter);
        }

        private class RelativePaths
        {
            public const string Login = "/login/";
        }
    }
}
