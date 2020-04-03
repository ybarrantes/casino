using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Casino.API.Filters
{
    public class ApiExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger = null;

        public int Order { get; set; } = int.MaxValue - 10;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                System.Net.HttpStatusCode defaultStatus = System.Net.HttpStatusCode.InternalServerError;
                WebApiResponse response = new WebApiResponse();
                System.Net.HttpStatusCode status = defaultStatus;

                if (context.Exception is WebApiException webApiException)
                {
                    status = (System.Net.HttpStatusCode)webApiException.Status;
                    response = response.Fail((System.Net.HttpStatusCode)webApiException.Status);
                } 
                else if (context.Exception is System.UnauthorizedAccessException unauthorizedException)
                {
                    status = System.Net.HttpStatusCode.Unauthorized;
                }

                string message = "";

                if (System.String.IsNullOrEmpty(message))
                {
                    message = (System.String.IsNullOrEmpty(context.Exception.Message)) ? status.ToString() : context.Exception.Message;
                }

                response = new WebApiResponse().Fail(status).SetMessage(message);

                context.Result = new ObjectResult(response){ StatusCode = (int)status };

                _logger.LogError(context.Exception, "Exception capturada en filtro");

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
