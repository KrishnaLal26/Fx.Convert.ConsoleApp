using System.Collections.Generic;
using System.Net;

namespace Fx.Convert.Framework
{
    public class Response<T> : Response
        where T : class
    {
        public T Data { get; set; }
    }

    public class Response
    {
        public HttpStatusCode ResponseCode { get; set; }

        public bool IsSuccessStatusCode => ((int)ResponseCode >= 200) && ((int)ResponseCode <= 299);

        public string ErrorMessage { get; set; }

        public IDictionary<string, string> ResponseHeaders { get; set; }
    }
}
