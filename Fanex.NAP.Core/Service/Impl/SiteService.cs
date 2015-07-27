namespace Fanex.NAP.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    public class SiteService : BaseService, ISiteService
    {
        public bool GetStatusById(int id)
        {
            return Get<bool>(string.Format(RelativePaths.GetStatusById, id));
        }

        public IEnumerable<SiteLevel> GetSiteLevelByUser(int userId, string env = Constant.Uat)
        {
            return Get<IEnumerable<SiteLevel>>(string.Format(RelativePaths.GetSiteLevelByUser, userId, env));
        }

        public IEnumerable<SiteUrl> GetSiteUrlBySiteId(int siteId, string env)
        {
            return Get<IEnumerable<SiteUrl>>(string.Format(RelativePaths.GetSiteUrlBySiteId, siteId, env));
        }

        private class RelativePaths
        {
            public const string GetStatusById = "/site/{0}/status";
            public const string GetSiteUrlBySiteId = "/site/{0}/siteUrl?environment={1}";
            public const string GetSiteLevelByUser = "/user/{0}/sitelevel?environment={1}";
        }
    }
}