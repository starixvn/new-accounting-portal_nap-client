namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Web.Handler;

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
