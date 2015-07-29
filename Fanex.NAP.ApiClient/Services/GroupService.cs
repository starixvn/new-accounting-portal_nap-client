namespace Fanex.NAP.Core.Services
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models;

    public class GroupService
    {
        private readonly GroupClient _groupClient;

        public GroupService()
        {
            _groupClient = ClientContext.Clients.CreateGroupClient();
        }

        public Group Get(int groupId)
        {
            Check.Argument.IsNotNegativeOrZero(groupId, "groupId");

            return _groupClient.Get(groupId);
        }
    }
}
