using ChatRoomServer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedContracts.Commands;
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
    public Task CreateRoom([FromBody] CreateRoomCommand createRoomCommand, CancellationToken cancellationToken)
    {
        logger.LogDebug("CreateRoom called with room name: {roomName}, room id: {roomId}", createRoomCommand.RoomName, createRoomCommand.RoomId);

        return mediator.Send(createRoomCommand, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> AddUserToRoom([FromBody] AddUserToRoomCommand addUserToRoomCommand, CancellationToken cancellationToken)
    {
        logger.LogDebug("AddUserToRoom called with room id: {roomId}, user ids: {userIds}", addUserToRoomCommand.RoomId, string.Join(", ", addUserToRoomCommand.AddedUserIds));

        try
        {
            await mediator.Send(addUserToRoomCommand, cancellationToken);
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
        var removeUserFromRoomCommand = new RemoveUserFromRoomCommand(roomId, userId, senderId);
        logger.LogDebug("RemoveUserFromRoom called with room id: {roomId}, user id: {userId}, sender id: {senderId}",
            removeUserFromRoomCommand.RoomId,
            removeUserFromRoomCommand.RemovedUserId,
            removeUserFromRoomCommand.SenderId);

        return mediator.Send(removeUserFromRoomCommand, cancellationToken);
    }
}
