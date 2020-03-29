using Casino.API.Util.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Exceptions
{
    public class HttpResponseException : Exception
    {
        protected string _message;

        public int Status { get; set; } = (int)System.Net.HttpStatusCode.InternalServerError;
        public HttpResponse Value { get; }
        public override string Message { get => _message;}

        public HttpResponseException(System.Net.HttpStatusCode Status)
        {
            this.Status = (int)Status;
            Value = new HttpResponse().Fail(Status);
            _message = Status.ToString();
        }

        public HttpResponseException(System.Net.HttpStatusCode Status, string DetailMessage) : this(Status)
        {
            Value.SetMessage(DetailMessage);
            _message = !String.IsNullOrEmpty(DetailMessage) ? DetailMessage : _message;
        }
    }
}
