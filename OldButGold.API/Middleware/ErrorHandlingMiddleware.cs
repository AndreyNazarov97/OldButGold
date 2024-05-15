﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using System;

namespace OldButGold.API.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(
            HttpContext httpContext, 
            ILogger<ErrorHandlingMiddleware> logger,
            ProblemDetailsFactory problemDetailsFactory)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Error has happened with {RequestPath}, the message is {ErrorMessage}",
                    httpContext.Request.Path.Value, exception.Message);

                ProblemDetails problemDetails;
                switch (exception)
                {
                    case IntentionManagerException intentionManagerException:
                        problemDetails = problemDetailsFactory.CreateFrom(httpContext, intentionManagerException);
                        break;
                    case ValidationException validationException:
                        problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                        logger.LogInformation(validationException, "Somebody sent invalid request, oops");
                        break;
                    case DomainException domainException:
                        problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                        logger.LogError(domainException, "Domain exception occured");
                        break;
                    default:
                        problemDetails = problemDetailsFactory.CreateProblemDetails(
                            httpContext,
                            StatusCodes.Status500InternalServerError,
                            "Unhadled error!");
                        logger.LogError(exception, "Unhadled exception occured");
                        break;

                }

                httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
            }
        }
    }
}
