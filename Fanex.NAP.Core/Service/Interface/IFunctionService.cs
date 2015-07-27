namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model.ValueObject;

    public interface IFunctionService : IBaseService
    {
        IEnumerable<FunctionPolicy> GetByUserAndSiteIncludeGroup(int userId, int siteId, string env = Constant.Uat);
    }
}