namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Service.Interface;

    public class GroupService : IGroupService
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