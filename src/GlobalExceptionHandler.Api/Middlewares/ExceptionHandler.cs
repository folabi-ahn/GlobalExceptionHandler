using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace GlobalExceptionHandler.Api;

internal sealed class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    private const string UnhandledExceptionMessage = "An unhandled exception has occurred while executing the request.";
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"Exception occured : {exception.Message}");
        
        int statusCode = GetStatusCodeFromException(exception);
        string problemType = GetProblemTypeFromException(exception);
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode) ?? UnhandledExceptionMessage;

        var result = new ProblemDetails
        {
            Status = statusCode,
            Type = problemType,
            Title = reasonPhrase,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
        return true;
    }

    private int GetStatusCodeFromException(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

    private string GetProblemTypeFromException(Exception exception) =>
        exception switch
        {
            ValidationException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            NotFoundException => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            _ => exception.GetType().Name
        };
}
