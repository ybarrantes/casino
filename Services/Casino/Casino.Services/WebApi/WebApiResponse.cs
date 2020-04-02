using System;
using System.Collections.Generic;

namespace Casino.Services.WebApi
{
    public class WebApiResponse : IWebApiResponse
    {
        protected int _status = (int)System.Net.HttpStatusCode.InternalServerError;
        protected string _title = "";
        protected object _data = new object { };
        protected List<KeyValuePair<string, object>> _errors = new List<KeyValuePair<string, object>>();

        public int Status => _status;
        public string Title { get => _title; }
        public object Data { get => _data; }
        public List<KeyValuePair<string, object>> Errors { get => _errors; }

        public WebApiResponse Success()
        {
            SetStatusCode(System.Net.HttpStatusCode.OK);
            return this;
        }

        public WebApiResponse Fail(System.Net.HttpStatusCode httpStatusCode)
        {
            if ((int)httpStatusCode == (int)System.Net.HttpStatusCode.OK)
                throw new ArgumentOutOfRangeException("httpStatusCode", httpStatusCode, "");

            SetStatusCode(httpStatusCode);
            return this;
        }

        public WebApiResponse SetMessage(string Message)
        {
            _title = Message;
            return this;
        }

        public WebApiResponse SetData(object Data)
        {
            _data = Data;
            return this;
        }

        // TODO: hay que hacer una mejora para visualizar de una forma mas clara los errores
        public WebApiResponse AddError(string key, object value)
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
