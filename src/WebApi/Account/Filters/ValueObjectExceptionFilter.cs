using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.WebApi.Account.Filters;

public class ValueObjectExceptionFilter(ILogger<ValueObjectExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            // Handle direct ValueObjectException
            case ValueObjectException valueObjectException:
                HandleValueObjectException(context, valueObjectException);
                return;
            // Handle wrapped ValueObjectException (from reflection)
            case TargetInvocationException { InnerException: ValueObjectException wrappedValueObjectException }:
                HandleValueObjectException(context, wrappedValueObjectException);
                break;
        }
    }

    private void HandleValueObjectException(ExceptionContext context, ValueObjectException exception)
    {
        logger.LogWarning(exception,
            "Value object validation failed: {Message}",
            exception.Message);

        var problemDetails = CreateValidationProblemDetails(exception, context);

        context.Result = new BadRequestObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        ValueObjectException exception,
        ExceptionContext context)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Value Object Validation Error",
            Detail = "One or more value objects contain invalid data.",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path,
            Extensions =
            {
                // Add traceId for debugging
                ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
            }
        };

        // If we have property information, add it to validation errors
        if (!string.IsNullOrEmpty(exception.PropertyName))
            problemDetails.Errors.Add(exception.PropertyName, [exception.Message]);
        else
            // If no specific property, add as general validation error
            problemDetails.Errors.Add("", [exception.Message]);

        return problemDetails;
    }
}