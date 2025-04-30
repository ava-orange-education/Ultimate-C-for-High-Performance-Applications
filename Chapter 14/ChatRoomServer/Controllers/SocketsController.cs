using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomServer.Controllers;

[ApiController]
public class SocketsController(ILogger<SocketsController> logger,
    IUserStore userStore,
    IMediator mediator) : ControllerBase
{
    [Route("/ws")]
    public async Task HandleConnection([FromQuery] Guid userId)
    {
        logger.LogDebug("WebSocket connection requested for user: {userId}.", userId);

        //The user must be logged in before conneting to the socket.
        if (userStore.CheckUserExists(userId) == false)
        {
            logger.LogDebug("User not found: {userId}.", userId);
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        //The cancellation token is unused in a web socket scenario.
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            logger.LogDebug("WebSocket connection accepted for user: {userId}.", userId);

            //This will not return until the socket is closed
            await mediator.Publish(new SocketConnectedEvent(userId, webSocket));
        }
        else
        {
            logger.LogDebug("WebSocket request is not valid.");
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
