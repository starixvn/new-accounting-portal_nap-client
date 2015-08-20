namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Models.ValueObject;

    public interface ISiteService
    {
        IEnumerable<SiteUrl> GetUrlById(int siteId, string environment);

        bool IsActivated(int siteId);
    }
}
