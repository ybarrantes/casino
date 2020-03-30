using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Util.Response
{
    public class HttpResponse : IHttpResponse
    {
        protected int _status = (int)System.Net.HttpStatusCode.InternalServerError;
        protected string _title = "";
        protected object _data = new object { };
        protected List<KeyValuePair<string, object>> _errors = new List<KeyValuePair<string, object>>();

        public int status => _status;
        public object data { get => _data; }
        public string title { get => _title; }
        public List<KeyValuePair<string, object>> errors { get => _errors; }

        public HttpResponse Success()
        {
            SetStatusCode(System.Net.HttpStatusCode.OK);
            return this;
        }

        public HttpResponse Fail(System.Net.HttpStatusCode httpStatusCode)
        {
            if ((int)httpStatusCode == (int)System.Net.HttpStatusCode.OK)
                throw new ArgumentOutOfRangeException("httpStatusCode", httpStatusCode, "");

            SetStatusCode(httpStatusCode);
            return this;
        }

        public HttpResponse SetMessage(string Message)
        {
            _title = Message;
            return this;
        }

        public HttpResponse SetData(object Data)
        {
            _data = Data;
            return this;
        }

        // TODO: hay que hacer una mejora para visualizar de una forma mas clara los errores
        public HttpResponse AddError(string key, object value)
        {
            _errors.Add(new KeyValuePair<string, object>(key, value));
            return this;
        }

        private void SetStatusCode(System.Net.HttpStatusCode httpStatusCode)
        {
            _status = (int)httpStatusCode;
            _title = httpStatusCode.ToString();
        }
    }
}
