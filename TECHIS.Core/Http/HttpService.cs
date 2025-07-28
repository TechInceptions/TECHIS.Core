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

        public async Task<HttpResult<byte[]>> GetBytes( Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null)
        {
            using (var response = await GetResponse(uri, httpMethod, contentType, headers, postBody))
            {
                var body = await GetContentAsByteArray(response);
                var mediaType = GetContentType(response, body);

                HttpResult<byte[]> result;
                if (response?.IsSuccessStatusCode == true)
                {
                    result = new HttpResult<byte[]>(body, response.StatusCode, response.ReasonPhrase, true)
                    {
                        ContentType = mediaType
                    };
                }
                else
                {
                    result = new HttpResult<byte[]>(body, response.StatusCode, response.ReasonPhrase, false)
                    {
                        ContentType = mediaType,                                // <-- set for failures too
                        Message = await GetContentAsString(response)
                    };
                }

                // length property, set it here:
                result.ContentLength = response.Content?.Headers?.ContentLength ?? body?.LongLength ?? 0L;

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

        private static string GetContentType(HttpResponseMessage response, byte[] body)
        {
            var mediaType = response.Content?.Headers?.ContentType?.MediaType;

            if (string.IsNullOrWhiteSpace(mediaType))
            {
                if (response.Content?.Headers?.TryGetValues("Content-Type", out var vals) == true)
                {
                    var raw = vals.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(raw))
                        mediaType = raw.Split(';')[0].Trim(); // drop params if any
                }
            }

            if (string.IsNullOrWhiteSpace(mediaType))
            {
                mediaType = DetectMimeFromBytes(body) ?? "application/octet-stream";
            }
            return mediaType;
        }
        private static string DetectMimeFromBytes(byte[] b)
        {
            if (b == null || b.Length < 12)
                return null;

            // JPEG FF D8 FF
            if (b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF)
                return "image/jpeg";

            // PNG 89 50 4E 47 0D 0A 1A 0A
            if (b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47 && b[4] == 0x0D && b[5] == 0x0A && b[6] == 0x1A && b[7] == 0x0A)
                return "image/png";

            // GIF "GIF87a"/"GIF89a"
            if (b.Length >= 6 && b[0] == 0x47 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x38 && (b[4] == 0x39 || b[4] == 0x37) && b[5] == 0x61)
                return "image/gif";

            // WEBP: "RIFF....WEBP"
            if (b.Length >= 12 && b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46 && b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50)
                return "image/webp";

            // AVIF: ftyp avif/avis/avif
            if (b.Length >= 12 && b[4] == 0x66 && b[5] == 0x74 && b[6] == 0x79 && b[7] == 0x70 &&
                (b[8] == 0x61 && b[9] == 0x76 && b[10] == 0x69 && (b[11] == 0x66 || b[11] == 0x73)))
                return "image/avif";

            // SVG (text) – crude but effective
            var text = System.Text.Encoding.UTF8.GetString(b, 0, Math.Min(b.Length, 256)).TrimStart('\uFEFF', ' ', '\t', '\r', '\n');
            if (text.StartsWith("<svg", StringComparison.OrdinalIgnoreCase))
                return "image/svg+xml";

            return null;
        }

    }
}
