namespace Fanex.NAP.Core.Clients
{
    using Fanex.NAP.Core.Models.ParameterObject;
    using System;
    using System.Net.Http;

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
            return Send<SSOLoginParameter, int>(CreateRequestUri(RelativePaths.Login), HttpMethod.Post, parameter);
        }

        private class RelativePaths
        {
            public const string GetTime = "/ssologin/";
            public const string Login = "/ssologin/";
        }
    }
}