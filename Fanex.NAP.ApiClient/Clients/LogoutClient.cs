namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Web.Handler;

    internal class LogoutClient : BaseClient
    {
        public LogoutClient(Uri baseEndpoint, DelegatingHandler handler)
            : base(baseEndpoint, handler)
        {
        }

        public void Logout(int onlineUserId)
        {
            Send<int>(CreateRequestUri(RelativePaths.Logout), HttpMethod.Post, onlineUserId);
        }

        private class RelativePaths
        {
            public const string Logout = "/logout/";
        }
    }
}
