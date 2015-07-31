namespace Fanex.NAP.Core.Configuration
{
    using System.Configuration;

    public class DomainElement : ConfigurationElement
    {
        private const string DomainName = "Name";
        private const string DomainPath = "Path";

        [ConfigurationProperty(DomainName, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[DomainName];
            }

            set
            {
                this[DomainName] = value;
            }
        }

        [ConfigurationProperty(DomainPath, IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this[DomainPath];
            }

            set
            {
                this[DomainPath] = value;
            }
        }
    }
}