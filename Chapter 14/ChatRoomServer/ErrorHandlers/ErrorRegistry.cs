using Microsoft.AspNetCore.Mvc;

namespace ChatRoomServer.ErrorHandlers;

public class ErrorRegistry
{
    private static readonly Dictionary<ErrorType, ProblemDetails> ErrorMappings = new()
    {
        { ErrorType.UnknownError, new ProblemDetails { Status = StatusCodes.Status500InternalServerError, Title = "Internal Server Error", Detail = "An internal server error occurred. Please try again later." } },
        { ErrorType.UserNotFound, new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "User Not Found", Detail = "The requested user id could not be found." } },
        { ErrorType.InvalidUserName, new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Invalid User Name", Detail = "The user name cannot be blank or empty." } },
        { ErrorType.RoomIdNotFound, new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Room Not Found", Detail = "The specified room id was not found." } }
    };

    public static ProblemDetails GetProblemDetails(ErrorType errorType, string instance)
    {
        var problemDetails = ErrorMappings.GetValueOrDefault(errorType, ErrorMappings[ErrorType.UnknownError]);
        problemDetails.Instance = instance;
        return problemDetails;
    }

}
