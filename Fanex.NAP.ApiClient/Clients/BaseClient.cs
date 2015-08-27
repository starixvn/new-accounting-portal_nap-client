namespace Fanex.NAP.Core.Clients
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

        public BaseClient(Uri baseEndPoint, HttpClient httpClient)
        {
            BaseAddress = baseEndPoint;

            _httpClient = httpClient;
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
                if (respone.Result.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var content = respone.Result.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        result = JsonConvert.DeserializeObject<T>(content);
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException(string.Format("You do not have permission to access this link: {0}", requestUri));
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
            var uriBuilder = new UriBuilder(BaseAddress + relativePath) { Query = queryString };

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