namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Handler;
    using Newtonsoft.Json;

    public class BaseService : IBaseService
    {
        public IServiceContext ServiceContext { get; set; }

        public T Get<T>(string endPoint)
        {
            T result = default(T);
            NAPDelegatingHandler customDelegatingHandler = new NAPDelegatingHandler();

            using (var httpClient = HttpClientFactory.Create(customDelegatingHandler))
            {
                httpClient.BaseAddress = new Uri(NapSettings.Settings.ServiceUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponeMessage = httpClient.GetAsync(httpClient.BaseAddress + endPoint).Result;
                if (httpResponeMessage.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<T>(httpResponeMessage.Content.ReadAsStringAsync().Result);
                }
            }

            return result;
        }

        public TResult Post<TResult, TData>(TData data, string endPoint)
        {
            TResult result = default(TResult);
            NAPDelegatingHandler customDelegatingHandler = new NAPDelegatingHandler();

            using (var httpClient = HttpClientFactory.Create(customDelegatingHandler))
            {
                httpClient.BaseAddress = new Uri(NapSettings.Settings.ServiceUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var httpResponeMessage = httpClient.PostAsJsonAsync(httpClient.BaseAddress + endPoint, data).Result;
                if (httpResponeMessage.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<TResult>(httpResponeMessage.Content.ReadAsStringAsync().Result);
                }
            }

            return result;
        }
    }
}