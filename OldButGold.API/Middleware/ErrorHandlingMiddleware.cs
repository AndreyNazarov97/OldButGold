using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace OldButGold.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext, 
            CancellationToken cancellationToken)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                var httpStatusCode = exception switch
                {
                    IntentionManagerException => StatusCodes.Status403Forbidden,
                    ValidationException => StatusCodes.Status400BadRequest,
                    DomainException domainException => domainException.ErrorCode switch
                    {
                        ErrorCode.Gone => StatusCodes.Status410Gone,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    _ => StatusCodes.Status500InternalServerError
                };
                httpContext.Response.StatusCode = httpStatusCode;
            }
        }
    }
}
