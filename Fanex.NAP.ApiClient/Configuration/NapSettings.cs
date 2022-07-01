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
        private const string NAPIsNapHub = "IsNapHub";
        private const string NAPEnableOtp = "EnableOtp";

        private static NapSettings _settings = ConfigurationManager.GetSection(NAPSettings) as NapSettings;

        public static NapSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public bool EnableDirectLogin
        {
            get
            {
                return ToBoolean(_EnableDirectLogin);
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
                return ToBoolean(_EnableReCheckIn);
            }

            set
            {
                _EnableReCheckIn = value.ToString();
            }
        }


        public bool IsNapHub
        {
            get
            {
                return ToBoolean(_IsNapHub);
            }

            set
            {
                _IsNapHub = value.ToString();
            }
        }

        public bool EnableOtp
        {
            get
            {
                return ToBoolean(_EnableOtp);
            }

            set
            {
                _EnableOtp = value.ToString();
            }
        }

        private bool ToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.ToLower();
            return value == "true" || value == "1";
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

        [ConfigurationProperty(NAPIsNapHub, IsRequired = false)]
        private string _IsNapHub
        {
            get { return (string)this[NAPIsNapHub]; }
            set { this[NAPIsNapHub] = value; }
        }

        [ConfigurationProperty(NAPEnableOtp, IsRequired = false)]
        private string _EnableOtp
        {
            get { return (string)this[NAPEnableOtp]; }
            set { this[NAPEnableOtp] = value; }
        }
    }
}