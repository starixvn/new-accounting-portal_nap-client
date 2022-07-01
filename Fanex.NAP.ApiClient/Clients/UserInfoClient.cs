namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;

    internal class UserInfoClient : BaseClient
    {
        public UserInfoClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public bool UpdatePassword(int siteId, int userId, string password)
        {
            var parameters = new
            {
                siteId,
                userId,
                password
            };
            return Get<bool>(CreateRequestUri(RelativePaths.UpdatePassword, parameters));
        }

        private static class RelativePaths
        {
            public const string UpdatePassword = "userinfo/updatePassword";
        }
    }
}