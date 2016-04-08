namespace Fanex.NAP.Core.Clients
{
    using Fanex.NAP.Core.Models;
    using System;
    using System.Net.Http;

    internal class GroupClient : BaseClient
    {
        public GroupClient(Uri baseEndpoint, HttpClient httpClient)
            : base(baseEndpoint, httpClient)
        {
        }

        public Group Get(int groupId)
        {
            return Get<Group>(CreateRequestUri(string.Format(RelativePaths.Get, groupId)));
        }

        private class RelativePaths
        {
            public const string Get = "/group/{0}";
        }
    }
}