using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ToDo.API.Filters
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception, context.Exception.Message);

            var exception = context.Exception;

            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Detail = exception.Message,
                Instance = context.HttpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };

            context.ExceptionHandled = true;
        }
    }
}
