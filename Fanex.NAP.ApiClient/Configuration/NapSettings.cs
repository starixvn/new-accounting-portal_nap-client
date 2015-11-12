namespace Fanex.NAP.Core.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    public class NapSettings : ConfigurationSection
    {
        private const string NAPSettings = "NapSettings";
        private const string NAPEnvironment = "Environment";
        private const string NAPEnableDirectLogin = "EnableDirectLogin";
        private const string NAPHubUrl = "HubUrl";
        private const string NAPSiteId = "SiteId";
        private const string NAPUmAddress = "UMAddress";
        private const string NAPEnableCheckUm = "EnableCheckUM";
        private const string NAPEnableReCheckin = "EnableReCheckin";
        private const string NAPUnauthorizeUrl = "UnauthorizeUrl";
        private const string NAPServiceUrls = "ServiceUrls";
        private const string NAPAccessKey = "AccessKey";
        private const string NAPDomain = "Domain";
        private const string NAPDomainAdd = "add";
        private const string NAPCacheDuration = "CacheDuration";
        private const string NAPUseWindowsAuthentication = "UseWindowsAuthentication";

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

        public bool EnableCheckUM
        {
            get
            {
                var result = false;

                if (!bool.TryParse(this._EnableCheckUM, out result))
                {
                    result = Equals(this._EnableCheckUM, "1");
                }

                return result;
            }

            set
            {
                this._EnableCheckUM = value.ToString();
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

        [ConfigurationProperty(NAPUmAddress, IsRequired = true)]
        public string UMAddress
        {
            get { return (string)this[NAPUmAddress]; }
            set { this[NAPUmAddress] = value; }
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

        [ConfigurationProperty(NAPAccessKey, IsRequired = true)]
        public string AccessKey
        {
            get { return (string)this[NAPAccessKey]; }
            set { this[NAPAccessKey] = value; }
        }

        [ConfigurationProperty(NAPCacheDuration, IsRequired = false, DefaultValue = 0)]
        public int CacheDuration
        {
            get { return (int)this[NAPCacheDuration]; }
            set { this[NAPCacheDuration] = value; }
        }

        [ConfigurationProperty(NAPUseWindowsAuthentication, IsRequired = false, DefaultValue = false)]
        public bool UseWindowsAuthentication
        {
            get { return (bool)this[NAPUseWindowsAuthentication]; }
            set { this[NAPUseWindowsAuthentication] = value; }
        }

        [ConfigurationProperty(NAPEnableDirectLogin, IsRequired = true)]
        private string _EnableDirectLogin
        {
            get { return (string)this[NAPEnableDirectLogin]; }
            set { this[NAPEnableDirectLogin] = value; }
        }

        [ConfigurationProperty(NAPEnableCheckUm, IsRequired = true)]
        private string _EnableCheckUM
        {
            get { return (string)this[NAPEnableCheckUm]; }
            set { this[NAPEnableCheckUm] = value; }
        }

        [ConfigurationProperty(NAPEnableReCheckin, IsRequired = true)]
        private string _EnableReCheckIn
        {
            get { return (string)this[NAPEnableReCheckin]; }
            set { this[NAPEnableReCheckin] = value; }
        }
    }
}