using MediatR;
using System.Text.Json.Serialization;

namespace SharedContracts.Commands;

public class AddUserToRoomCommand : IRequest<Unit>
{
    [JsonConstructor]
    public AddUserToRoomCommand(Guid roomId, Guid senderId, Guid[] addedUserIds)
    {
        RoomId = roomId;
        SenderId = senderId;
        AddedUserIds = addedUserIds;
    }

    public Guid RoomId { get; }

    public Guid SenderId { get; }

    public Guid[] AddedUserIds { get; }
}
