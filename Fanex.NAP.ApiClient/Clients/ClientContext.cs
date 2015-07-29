namespace Fanex.NAP.Core.Clients
{
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web.Handler;

    internal static class ClientContext
    {
        static ClientContext()
        {
            Configuration = NapSettings.Settings;
            Clients = new NAPClients
            {
                CreateDelegatingHandler = () => new NAPDelegatingHandler(Configuration.SiteId.ToString(), Configuration.AccessKey)
            };
        }

        public static NAPClients Clients { get; private set; }

        public static NapSettings Configuration { get; private set; }
    }
}
