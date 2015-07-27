namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Interface;

    public class GroupService : BaseService, IGroupService
    {
        public IGroup GetById(int id)
        {
            return Get<Group>(string.Format(RelativePaths.GetById, id));
        }

        private class RelativePaths
        {
            public const string GetById = "/group/{0}";
        }
    }
}