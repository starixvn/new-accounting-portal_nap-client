namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    public class FunctionService : BaseService, IFunctionService
    {
        public IEnumerable<FunctionPolicy> GetByUserAndSiteIncludeGroup(int userId, int siteId, string env = Constant.Uat)
        {
            return Get<IEnumerable<FunctionPolicy>>(string.Format(RelativePaths.GetByUserAndSiteIncludeGroup, userId, siteId, env));
        }

        private class RelativePaths
        {
            public const string GetByUserAndSiteIncludeGroup = "/user/{0}/functionPolicy?siteId={1}&environment={2}";
        }
    }
}