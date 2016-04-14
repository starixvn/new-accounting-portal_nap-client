namespace Fanex.NAP.Core.Clients
{
    using Models.ParameterObject;
    using System;
    using System.Net.Http;

    internal class UserInfoClient : BaseClient
    {
        public UserInfoClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public bool UpdatePassword(int userId, string password)
        {
            return Send<object, bool>(CreateRequestUri(RelativePaths.UpdatePassword, new { userId = userId, password = password }), HttpMethod.Post, null);
        }

        public bool UpdateProfile(UpdateUserInfoParameter parameter)
        {
            return Send<UpdateUserInfoParameter, bool>(CreateRequestUri(RelativePaths.UpdateProfile), HttpMethod.Post, parameter);
        }

        private class RelativePaths
        {
            public const string UpdatePassword = "/userinfo/updatePassword";
            public const string UpdateProfile = "/userinfo/updateProfile";
        }
    }
}