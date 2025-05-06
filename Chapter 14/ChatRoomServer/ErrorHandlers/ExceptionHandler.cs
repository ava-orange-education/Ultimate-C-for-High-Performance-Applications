using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomServer.ErrorHandlers;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        if (exception is StatusCodeException statusEx)
        {
            problemDetails = statusEx.ProblemDetails;
            httpContext.Response.StatusCode = problemDetails.Status!.Value;
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception?.Message ?? "An unknown error occurred",
                Instance = httpContext.Request.Path
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Indicate that the exception was handled
    }

}
