using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;

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
            ProblemDetailsFactory problemDetailsFactory)
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
                var problemDetails = exception switch
                {
                    IntentionManagerException intentionManagerException => 
                        problemDetailsFactory.CreateFrom(httpContext, intentionManagerException),
                    ValidationException validationException =>
                        problemDetailsFactory.CreateFrom(httpContext, validationException),
                    DomainException domainException =>
                        problemDetailsFactory.CreateFrom(httpContext, domainException),
                    _ => problemDetailsFactory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError,
                        "Unhadled error!", detail : exception.Message)
                };
                httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
