using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Util.Response
{
    interface IHttpResponse
    {
        public int status { get; }
        public string title { get; }
        public object data { get;  }
        public List<KeyValuePair<string, object>> errors { get; }
    }
}
