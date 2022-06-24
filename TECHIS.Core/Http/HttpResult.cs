using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public class HttpResult<TBody>
    {
        public readonly TBody Body;
        public readonly HttpStatusCode StatusCode;
        public readonly string Reason;
        public readonly bool Succeeded;
        public string Message { get; set; }
        public string InitialRequestBody { get; set; }
        public string ContentType { get; set; }

        public HttpResult(TBody body, HttpStatusCode statusCode, string reason, bool succeeded)
        {
            Body = body;
            StatusCode = statusCode;
            Reason = reason;
            Succeeded = succeeded;
        }

        public bool IsOkResult()
        {
            bool val = false;
            switch (StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.Continue:
                case HttpStatusCode.Created:
                case HttpStatusCode.OK:
                    val = true;
                    break;
                default:
                    break;
            }

            return val;
        }
    }
}
