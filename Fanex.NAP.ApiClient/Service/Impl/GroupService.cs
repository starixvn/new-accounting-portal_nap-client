namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Service.Interface;

    public class GroupService : IGroupService
    {
        public Group Get(int groupId)
        {
            Check.Argument.IsNotNegativeOrZero(groupId, "groupId");
            using (var groupClient = ClientContext.Clients.CreateGroupClient())
            {
                return groupClient.Get(groupId);
            }                
        }
    }
}