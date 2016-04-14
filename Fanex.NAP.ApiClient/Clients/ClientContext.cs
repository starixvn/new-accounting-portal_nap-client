namespace Fanex.NAP.Core.Clients
{
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Exceptions;
    using Fanex.NAP.Core.Web.Handler;
    using System;
    using System.Linq;
    using System.Net.Http;

    internal static class ClientContext
    {
        private static Random _randomHandler = new Random();

        static ClientContext()
        {
            Configuration = NapSettings.Settings;
            Clients = new NAPClients
            {
                CreateHttpClient = CreateHttpClient
            };
        }

        public static NAPClients Clients { get; internal set; }

        public static NapSettings Configuration { get; internal set; }

        private static HttpClient CreateHttpClient()
        {
            return HttpClientFactory.Create(new NAPDelegatingHandler(Configuration.SiteId.ToString(), Configuration.AccessKey));
        }

        public static string GetServiceUrl()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ServiceUrls))
            {
                throw new NAPApiException("Service Urls cannot be null or white space");
            }

            var serviceUrls = Configuration.ServiceUrls.Split(',');
            var numOfUrl = serviceUrls.Count();

            if (numOfUrl == 1)
            {
                return serviceUrls[0];
            }

            var selectedServiceUrl = serviceUrls[_randomHandler.Next(0, numOfUrl)];

            return selectedServiceUrl;
        }
    }
}