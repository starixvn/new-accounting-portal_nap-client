namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Exceptions;
    using Extensions;
    using Newtonsoft.Json;

    internal class BaseClient
    {
        private readonly HttpClient httpClient;

        public BaseClient(Uri baseEndPoint, HttpClient httpClient)
        {
            BaseAddress = baseEndPoint;

            this.httpClient = httpClient;
        }

        public Uri BaseAddress { get; set; }

        protected T Get<T>(Uri requestUri)
        {
            T result = default(T);

            using (var message = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (var response = httpClient.SendAsync(message).Result)
                {
                    result = GetResponse<T>(response).Result;
                }
            }

            return result;
        }

        private static async Task<T> GetResponse<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new NAPApiException($"No permission");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new NAPApiException(await response.Content.ReadAsStringAsync());
            }
            else
            {
                var receivedContent = await response.Content.ReadAsStringAsync();

                return DeserializeObject<T>(receivedContent);
            }
        }

        private static T DeserializeObject<T>(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                throw new FormatException($"=======Received:===\r\n {content}\r\n========", e);
            }
        }

        protected TOutput Post<TInput, TOutput>(Uri requestUri, TInput body)
        {
            var json = JsonConvert.SerializeObject(body);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (var response = httpClient.PostAsync(requestUri, content).Result)
                {
                    ThrowIfResponseNotSuccessfulAsync(response);
                    var receivedContent = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        return JsonConvert.DeserializeObject<TOutput>(receivedContent);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"=======Received:===\r\n {receivedContent}\r\n========", e);
                    }
                }
            }
        }

        protected Uri CreateRequestUri(string relativePath, params KeyValuePair<string, string>[] queryStringParameters)
        {
            var queryString = string.Empty;

            if (queryStringParameters != null && queryStringParameters.Length > 0)
            {
                var queryStringProperties = BaseAddress.ParseQueryString();
                foreach (var queryStringParameter in queryStringParameters)
                {
                    queryStringProperties[queryStringParameter.Key] = queryStringParameter.Value;
                }

                queryString = queryStringProperties.ToString();
            }

            return CreateRequestUri(relativePath, queryString);
        }

        protected Uri CreateRequestUri(string relativePath, object queryStringParameters)
        {
            var parameters = queryStringParameters.ToPropertyDictionary()
                    .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToNullOrString()))
                    .ToArray();
            return CreateRequestUri(relativePath, parameters);
        }

        protected Uri CreateRequestUri(string relativePath, string queryString)
        {
            var uriBuilder = new UriBuilder(BaseAddress + relativePath) { Query = queryString };

            return uriBuilder.Uri;
        }

        private void ThrowIfResponseNotSuccessfulAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                // Do not treat not found as exception, but rather as null result
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
    }
}