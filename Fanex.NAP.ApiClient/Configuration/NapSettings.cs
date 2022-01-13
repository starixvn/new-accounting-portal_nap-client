namespace Fanex.NAP.Core.Configuration
{
    using System.Configuration;

    public class NapSettings : ConfigurationSection
    {
        private const string NAPSettings = "NapSettings";
        private const string NAPEnvironment = "Environment";
        private const string NAPEnableDirectLogin = "EnableDirectLogin";
        private const string NAPHubUrl = "HubUrl";
        private const string NAPSiteId = "SiteId";
        private const string NAPEnableReCheckin = "EnableReCheckin";
        private const string NAPUnauthorizeUrl = "UnauthorizeUrl";
        private const string NAPServiceUrls = "ServiceUrls";
        private const string NAPDomain = "Domain";
        private const string NAPDomainAdd = "add";
        private const string NAPCacheDuration = "CacheDuration";

        private static NapSettings _settings = ConfigurationManager.GetSection(NAPSettings) as NapSettings;

        public static NapSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public bool IsNapHub
        {
            get { return SiteId == 0; }
        }

        public bool EnableDirectLogin
        {
            get
            {
                var result = false;

                if (!bool.TryParse(_EnableDirectLogin, out result))
                {
                    result = Equals(_EnableDirectLogin, "1");
                }

                return result;
            }

            set
            {
                _EnableDirectLogin = value.ToString();
            }
        }

        public bool EnableReCheckIn
        {
            get
            {
                var result = false;

                if (!bool.TryParse(_EnableReCheckIn, out result))
                {
                    result = Equals(_EnableReCheckIn, "1");
                }

                return result;
            }

            set
            {
                _EnableReCheckIn = value.ToString();
            }
        }

        [ConfigurationProperty(NAPSiteId, IsRequired = true)]
        public int SiteId
        {
            get { return (int)this[NAPSiteId]; }
            set { this[NAPSiteId] = value; }
        }

        [ConfigurationProperty(NAPHubUrl, IsRequired = true)]
        public string HubUrl
        {
            get { return (string)this[NAPHubUrl]; }
            set { this[NAPHubUrl] = value; }
        }

        [ConfigurationProperty(NAPEnvironment, IsRequired = true)]
        public string Environment
        {
            get { return (string)this[NAPEnvironment]; }
            set { this[NAPEnvironment] = value; }
        }

        [ConfigurationProperty(NAPUnauthorizeUrl, IsRequired = false)]
        public string UnauthorizeUrl
        {
            get { return (string)this[NAPUnauthorizeUrl]; }
            set { this[NAPUnauthorizeUrl] = value; }
        }

        [ConfigurationProperty(NAPDomain, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DomainCollection), AddItemName = NAPDomainAdd)]
        public DomainCollection Domains
        {
            get { return (DomainCollection)base[NAPDomain]; }
        }

        [ConfigurationProperty(NAPServiceUrls, IsRequired = true)]
        public string ServiceUrls
        {
            get { return (string)this[NAPServiceUrls]; }
            set { this[NAPServiceUrls] = value; }
        }

        [ConfigurationProperty(NAPCacheDuration, IsRequired = false, DefaultValue = 0)]
        public int CacheDuration
        {
            get { return (int)this[NAPCacheDuration]; }
            set { this[NAPCacheDuration] = value; }
        }

        [ConfigurationProperty(NAPEnableDirectLogin, IsRequired = true)]
        private string _EnableDirectLogin
        {
            get { return (string)this[NAPEnableDirectLogin]; }
            set { this[NAPEnableDirectLogin] = value; }
        }

        [ConfigurationProperty(NAPEnableReCheckin, IsRequired = true)]
        private string _EnableReCheckIn
        {
            get { return (string)this[NAPEnableReCheckin]; }
            set { this[NAPEnableReCheckin] = value; }
        }
    }
}