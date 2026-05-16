using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDo.API.Filters
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
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
