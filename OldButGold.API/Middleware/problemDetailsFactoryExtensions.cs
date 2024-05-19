using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.Exceptions;


namespace OldButGold.Forums.API.Middleware
{
    public static class problemDetailsFactoryExtensions
    {
        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext httpContext,
            IntentionManagerException intentionManagerException)
        {
            return factory.CreateProblemDetails(
                httpContext,
                StatusCodes.Status403Forbidden,
                "Authorization failed",
                detail: intentionManagerException.Message);
        }
        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext httpContext,
            DomainException domainException)
        {
            return factory.CreateProblemDetails(
                httpContext,
                domainException.ErrorCode switch
                {
                    DomainErrorCode.Gone => StatusCodes.Status410Gone,
                    _ => StatusCodes.Status500InternalServerError,
                },
                domainException.Message);
        }

        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext httpContext,
            ValidationException validationException)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in validationException.Errors)
            {
                modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
            }

            return factory.CreateValidationProblemDetails(
                httpContext,
                modelStateDictionary,
                StatusCodes.Status400BadRequest,
                "Validation failed");
        }
    }
}
