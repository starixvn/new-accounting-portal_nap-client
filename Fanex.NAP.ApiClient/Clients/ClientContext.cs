namespace Fanex.NAP.Core.Clients
{
    using System.Net.Http;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web.Handler;

    internal static class ClientContext
    {
        static ClientContext()
        {
            Configuration = NapSettings.Settings;
            Clients = new NAPClients
            {
                CreateHttpClient = CreateHttpClient
            };
        }

        public static NAPClients Clients { get; private set; }

        public static NapSettings Configuration { get; private set; }

        private static HttpClient CreateHttpClient()
        {
            if (NapSettings.Settings.UseWindowsAuthentication)
            {
                return new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true
                });
            }
            else
            {
                return HttpClientFactory.Create(new NAPDelegatingHandler(Configuration.SiteId.ToString(), Configuration.AccessKey));
            }
        }
    }
}