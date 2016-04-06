namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Models.ParameterObject;

    internal class UserInfoClient : BaseClient
    {
        public UserInfoClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public bool UpdatePassword(UpdatePasswordParameter parameter)
        {
            return Send<UpdatePasswordParameter, bool>(CreateRequestUri(RelativePaths.UpdatePassword), HttpMethod.Post, parameter);
        }

        private class RelativePaths
        {
            public const string UpdatePassword = "/userinfo/updatePassword";
        }
    }
}
