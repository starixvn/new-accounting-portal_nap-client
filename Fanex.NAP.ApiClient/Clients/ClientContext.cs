namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Net.Http;
    using Configuration;
    using Exceptions;

    internal static class ClientContext
    {
        private static readonly Random RandomHandler = new Random();

        private static HttpClient httpClient;

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
            httpClient = httpClient ?? new HttpClient();
            return httpClient;
        }

        public static string GetServiceUrl()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ServiceUrls))
            {
                throw new NAPApiException("Service Urls cannot be null or white space");
            }

            var serviceUrls = Configuration.ServiceUrls.Split(',');
            var numOfUrl = serviceUrls.Length;

            if (numOfUrl == 1)
            {
                return serviceUrls[0];
            }

            var selectedServiceUrl = serviceUrls[RandomHandler.Next(0, numOfUrl)];

            return selectedServiceUrl;
        }
    }
}