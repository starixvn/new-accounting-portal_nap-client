namespace Fanex.NAP.Core.Service.Interface
{
    using Fanex.NAP.Core.Domain;

    public interface IGroupService : IBaseService
    {
        IGroup GetById(int id);
    }
}