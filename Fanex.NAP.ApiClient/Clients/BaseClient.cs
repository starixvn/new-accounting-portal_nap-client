namespace Fanex.NAP.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using Fanex.NAP.Core.Exceptions;
    using Fanex.NAP.Core.Extensions;
    using Fanex.NAP.Core.Web;
    using Newtonsoft.Json;

    internal class BaseClient
    {
        private readonly HttpClient _httpClient;

        public BaseClient(Uri baseEndPoint, HttpClient httpClient)
        {
            BaseAddress = baseEndPoint;

            _httpClient = httpClient;
        }

        public Uri BaseAddress { get; set; }

        protected T Get<T>(Uri requestUri)
        {
            T result = default(T);

            using (var message = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (var response = _httpClient.SendAsync(message))
                {
                    result = GetResponse(requestUri, result, response);
                }
            }

            return result;
        }

        private static T GetResponse<T>(Uri requestUri, T result, Task<HttpResponseMessage> response)
        {
            if (response.Result.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new NAPApiException(string.Format(ViewResources.Dont_Have_Permission, requestUri));
            }
            else if (response.Result.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new NAPApiException(response.Result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                var content = response.Result.Content.ReadAsStringAsync().Result;

                result = DeserializeResult(result, content);
            }

            return result;
        }

        private static T DeserializeResult<T>(T result, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<T>(content);
                }
                catch
                {
                    throw new NAPApiException(content);
                }
            }

            return result;
        }

        protected void Send<TInput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            Send<TInput, object>(requestUri, httpMethod, body);
        }

        protected TOutput Send<TInput, TOutput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            using (var message = new HttpRequestMessage(httpMethod, requestUri))
            {
                using (var objectContent = new ObjectContent<TInput>(body, CreateMediaTypeFormatter()))
                {
                    message.Content = new ObjectContent<TInput>(body, CreateMediaTypeFormatter());
                    using (var response = _httpClient.SendAsync(message))
                    {
                        ThrowIfResponseNotSuccessfulAsync(response.Result);
                        return response.Result.Content.ReadAsAsync<TOutput>().Result;
                    }
                }
            }
        }

        protected MediaTypeFormatter CreateMediaTypeFormatter()
        {
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            return formatter;
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