namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Models.ParameterObject;

    internal class SSOClient : BaseClient
    {
        public SSOClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public string GetTime()
        {
            return Get<string>(CreateRequestUri(RelativePaths.GetTime));
        }

        public int Login(SSOLoginParameter parameter)
        {
            return Post<SSOLoginParameter, int>(CreateRequestUri(RelativePaths.Login),  parameter);
        }

        private class RelativePaths
        {
            public const string GetTime = "ssologin";
            public const string Login = "ssologin";
        }
    }
}