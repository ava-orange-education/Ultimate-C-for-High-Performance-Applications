using ChatRoomServer.Events;
using ChatRoomServer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedContracts.Responses;

namespace ChatRoomServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(ILogger<RoomsController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<RoomResponse>> GetRoomsForUser([FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetRoomsForUser called with userId: {userId}", userId);

        var result = await mediator.Send(new GetRoomsQuery(userId), cancellationToken);
        return result;
    }

    [HttpPost]
    public Task CreateRoom([FromBody] RoomCreatedEvent roomCreatedEvent, CancellationToken cancellationToken)
    {
        logger.LogDebug("CreateRoom called with room name: {roomName}, room id: {roomId}", roomCreatedEvent.RoomName, roomCreatedEvent.RoomId);

        return mediator.Publish(roomCreatedEvent, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> AddUserToRoom([FromBody] UserAddedToRoomEvent userAddedToRoomEvent, CancellationToken cancellationToken)
    {
        logger.LogDebug("AddUserToRoom called with room id: {roomId}, user ids: {userIds}", userAddedToRoomEvent.RoomId, string.Join(", ", userAddedToRoomEvent.AddedUserIds));

        try
        {
            await mediator.Publish(userAddedToRoomEvent, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "User not found.");
            return NotFound($"User not found.");
        }
    }

    [HttpDelete]
    public Task RemoveUserFromRoom([FromQuery] Guid userId, [FromQuery] Guid roomId, [FromQuery] Guid senderId, CancellationToken cancellationToken)
    {
        var userRemovedFromRoomEvent = new UserRemovedFromRoomEvent(roomId, userId, senderId);
        logger.LogDebug("RemoveUserFromRoom called with room id: {roomId}, user id: {userId}, sender id: {senderId}",
            userRemovedFromRoomEvent.RoomId,
            userRemovedFromRoomEvent.UserId,
            userRemovedFromRoomEvent.SenderId);

        return mediator.Publish(userRemovedFromRoomEvent, cancellationToken);
    }
}
