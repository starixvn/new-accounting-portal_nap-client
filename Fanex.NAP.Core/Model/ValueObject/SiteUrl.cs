namespace Fanex.NAP.Core.Model.ValueObject
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Site URL model.
    /// </summary>
    public class SiteUrl
    {
        public int SiteUrlId { get; set; }

        public string Url { get; set; }

        public string Environment { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public Site Site { get; set; }
    }
}