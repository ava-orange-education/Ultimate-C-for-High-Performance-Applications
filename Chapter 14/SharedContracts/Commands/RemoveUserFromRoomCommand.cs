using MediatR;
using System.Text.Json.Serialization;

namespace SharedContracts.Commands;

public class RemoveUserFromRoomCommand : IRequest<Unit>
{
    [JsonConstructor]
    public RemoveUserFromRoomCommand(Guid roomId, Guid userId, Guid senderId)
    {
        RoomId = roomId;
        RemovedUserId = userId;
        SenderId = senderId;
    }

    public Guid RoomId { get; }
    public Guid RemovedUserId { get; }

    public Guid SenderId { get; }
}
