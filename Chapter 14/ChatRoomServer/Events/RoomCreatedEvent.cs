using MediatR;
using SharedContracts.Commands;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class RoomCreatedEvent : CreateRoomCommand, INotification
{
    [JsonConstructor]
    public RoomCreatedEvent(Guid roomId, string roomName, Guid[] userIds) :
        base(roomId, roomName, userIds)
    {
    }
}
