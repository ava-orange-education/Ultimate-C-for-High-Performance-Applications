
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomServer.ErrorHandlers;

internal class StatusCodeException : Exception
{
    public ProblemDetails ProblemDetails { get; }

    public StatusCodeException(ErrorType errorType, string instance)
        : base(ErrorRegistry.GetProblemDetails(errorType, instance).Detail)
    {
        ProblemDetails = ErrorRegistry.GetProblemDetails(errorType, instance);
    }

}