namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model.ValueObject;

    public interface ISiteService : IBaseService
    {
        bool GetStatusById(int id);

        IEnumerable<SiteLevel> GetSiteLevelByUser(int userId, string env = Constant.Uat);

        IEnumerable<SiteUrl> GetSiteUrlBySiteId(int siteId, string env);
    }
}