using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public class HttpService : IHttpService
    {
        private HttpClient _webClient;
        private TimeSpan? _OriginalTimeout;

        public HttpService(HttpClient webClient)
        {
            _webClient = webClient;
            PreserveOriginalTimeout();
        }
        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _webClient = httpClientFactory.CreateClient();
            PreserveOriginalTimeout();
        }
        public async Task<HttpResponseMessage> GetResponse(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null)
        {

            using (var request = new HttpRequestMessage())
            {
                request.RequestUri = uri;

                if (headers != null && headers.Count != 0)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }


                string url = uri.AbsoluteUri;
                if (httpMethod.Equals(HttpMethod.Get))
                {
                    request.Method = HttpMethod.Get;
                }
                else
                {
                    request.Content = new StringContent(postBody ?? String.Empty);
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType) { CharSet = Encoding.UTF8.HeaderName };
                    }

                    if (httpMethod.Equals(HttpMethod.Post))
                    {
                        request.Method = HttpMethod.Post;
                    }
                    else
                    if (httpMethod.Equals(HttpMethod.Put))
                    {
                        request.Method = HttpMethod.Put;
                    }
                    else
                    if (httpMethod.Equals(HttpMethod.Delete))
                    {
                        request.Method = HttpMethod.Delete;
                    }
                    else
                    {
                        throw new ArgumentException($"The http method '{httpMethod}' is not supported ", nameof(httpMethod));
                    }
                }
                var httpResponse = await _webClient.SendAsync(request);

                return httpResponse;
            }

        }

        public async Task<HttpResult<string>> GetBody(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null)
        {
            HttpResult<string> result;
            using (var response = await GetResponse(uri, httpMethod, contentType, headers, postBody))
            {
                var body = await GetContentAsString(response);
                if (response?.IsSuccessStatusCode == true)
                {
                    result = new HttpResult<string>(body, response.StatusCode, response.ReasonPhrase, true);
                }
                else
                {
                    result = new HttpResult<string>(body, response.StatusCode, response.ReasonPhrase, false);
                }

                return result;
            }
        }

        public async Task<HttpResult<byte[]>> GetBytes(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null)
        {
            HttpResult<byte[]> result;
            using (var response = await GetResponse(uri, httpMethod, contentType, headers, postBody))
            {
                var body = await GetContentAsByteArray(response);
                if (response?.IsSuccessStatusCode == true)
                {
                    result = new HttpResult<byte[]>(body, response.StatusCode, response.ReasonPhrase, true) { ContentType = response.Content.Headers.ContentType.MediaType  };
                }
                else
                {
                    result = new HttpResult<byte[]>(body, response.StatusCode, response.ReasonPhrase, false)
                    {
                        Message = await GetContentAsString(response)
                    };
                }

                return result;
            }
        }
        public static async Task<byte[]> GetContentAsByteArray(HttpResponseMessage response)
        {
            byte[] result;
            //check encoding
            if (response.Content.Headers?.ContentEncoding?.FirstOrDefault() == "gzip")
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                result= Decompress(data);
            }
            else
            {
                result = await response.Content.ReadAsByteArrayAsync();
            }

            return result;
        }
        public static async Task<string> GetContentAsString(HttpResponseMessage response)
        {
            string result;
            //check encoding
            if (response.Content.Headers?.ContentEncoding?.FirstOrDefault() == "gzip")
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                var encodedString = Decompress(data);
                result = Encoding.UTF8.GetString(encodedString);
            }
            else
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
        static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
        public static HttpMethod GetMethod(string method)
        {
            method = method.ToLowerInvariant();
            HttpMethod httpMethod;
            switch (method)
            {
                case "delete":
                    httpMethod = HttpMethod.Delete;
                    break;
                case "get":
                    httpMethod = HttpMethod.Get;
                    break;
                case "head":
                    httpMethod = HttpMethod.Head;
                    break;

                case "post":
                    httpMethod = HttpMethod.Post;
                    break;
                case "put":
                    httpMethod = HttpMethod.Put;
                    break;
                case "options":
                    httpMethod = HttpMethod.Options;
                    break;
                default:
                    httpMethod = HttpMethod.Get;
                    break;
            }
            return httpMethod;
        }

        public void SetTimeout(int timeoutInSeconds)
        {
            if (_webClient != null)
            {
                _webClient.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            }
        }

        private void PreserveOriginalTimeout()
        {
            _OriginalTimeout = _webClient?.Timeout;
        }
    }
}
