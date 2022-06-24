using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetResponse(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);

        Task<HttpResult<string>> GetBody(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);

        Task<HttpResult<byte[]>> GetBytes(Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);
    }
    public interface ICachedHttpService
    {
        Task<HttpResponseMessage> GetResponse(int cacheTimeInMintues, Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);

        Task<HttpResult<string>> GetBody(int cacheTimeInMintues, Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);

        Task<HttpResult<byte[]>> GetBytes(int cacheTimeInMintues, Uri uri, HttpMethod httpMethod, string contentType, IDictionary<string, string> headers, string postBody = null);
    }
}
