using System.Net;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Common.Exceptions;

namespace WorkPulse.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception");
            await WriteProblemDetailsAsync(context, exception);
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException => ((int)HttpStatusCode.BadRequest, "Validation failed."),
            NotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found."),
            ForbiddenException => ((int)HttpStatusCode.Forbidden, "Forbidden."),
            ConflictException => ((int)HttpStatusCode.Conflict, "Conflict."),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred."),
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Type = $"https://httpstatuses.com/{statusCode}",
            Instance = context.Request.Path,
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors;
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json");
    }
}
