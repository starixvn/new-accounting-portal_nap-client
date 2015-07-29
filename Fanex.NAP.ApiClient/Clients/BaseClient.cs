namespace Fanex.NAP.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using Fanex.NAP.Core.Extensions;
    using Newtonsoft.Json;

    internal class BaseClient
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public BaseClient(Uri baseEndPoint, DelegatingHandler handler = null)
        {
            BaseAddress = baseEndPoint;

            _httpClient = HttpClientFactory.Create(handler);
            _disposed = false;
        }

        public Uri BaseAddress { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected T Get<T>(Uri requestUri)
        {
            T result = default(T);

            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using (var respone = _httpClient.SendAsync(message))
            {
                result = JsonConvert.DeserializeObject<T>(respone.Result.Content.ReadAsStringAsync().Result);
            }

            return result;
        }

        protected void Send(Uri requestUri, HttpMethod httpMethod)
        {
            Send<object>(requestUri, httpMethod);
        }

        protected TOutput Send<TOutput>(Uri requestUri, HttpMethod httpMethod)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri);
            return Send<TOutput>(message, true);
        }

        protected void Send<TInput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            Send<TInput, object>(requestUri, httpMethod, body);
        }

        protected TOutput Send<TInput, TOutput>(Uri requestUri, HttpMethod httpMethod, TInput body)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = new ObjectContent<TInput>(body, CreateMediaTypeFormatter())
            };

            return Send<TOutput>(message, true);
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
            var endpoint = new Uri(BaseAddress, "/api" + relativePath);
            var uriBuilder = new UriBuilder(endpoint) { Query = queryString };
            return uriBuilder.Uri;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    _httpClient.Dispose();
                }
            }

            _disposed = true;
        }

        private TOutput Send<TOutput>(HttpRequestMessage message, bool hasResult)
        {
            using (var response = _httpClient.SendAsync(message))
            {
                ThrowIfResponseNotSuccessfulAsync(response.Result);

                if (!hasResult)
                {
                    return default(TOutput);
                }

                return response.Result.Content.ReadAsAsync<TOutput>().Result;
            }
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
