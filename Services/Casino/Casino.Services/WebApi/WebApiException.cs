
namespace Casino.Services.WebApi
{
    public class WebApiException : System.Exception
    {
        protected string _message;

        public int Status { get; set; } = (int)System.Net.HttpStatusCode.InternalServerError;
        public override string Message { get => _message;}

        public WebApiException(System.Net.HttpStatusCode status)
        {
            this.Status = (int)status;
            _message = Status.ToString();
        }

        public WebApiException(System.Net.HttpStatusCode status, string message) : this(status)
        {
            _message = !System.String.IsNullOrEmpty(message) ? message : _message;
        }
    }
}
