using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToDo.Domain.Common.Notification;

namespace ToDo.API.Filters
{
    public class DomainErrorsFilter : IAsyncActionFilter
    {
        private readonly DomainNotification _domainNotification;

        public DomainErrorsFilter(DomainNotification domainNotification)
        {
            _domainNotification = domainNotification;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (_domainNotification.HasErrors())
            {
                executedContext.Result = new BadRequestObjectResult(new
                {
                    errors = _domainNotification.Errors
                });
            }
        }
    }
}
