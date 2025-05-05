using MediatR;
using System.Text.Json.Serialization;

namespace SharedContracts.Commands;

public class CreateRoomCommand : IRequest<Unit>
{
    [JsonConstructor]
    public CreateRoomCommand(Guid roomId, string roomName, Guid[] userIds)
    {
        RoomId = roomId;
        RoomName = roomName;
        UserIds = userIds;
    }

    public Guid RoomId { get; }
    public string RoomName { get; }

    public Guid[] UserIds { get; }
}
