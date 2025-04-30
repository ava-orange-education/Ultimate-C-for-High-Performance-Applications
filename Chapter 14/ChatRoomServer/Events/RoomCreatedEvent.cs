using MediatR;
using SharedContracts.Events;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class RoomCreatedEvent : RoomCreated, INotification
{
    [JsonConstructor]
    public RoomCreatedEvent(Guid roomId, string roomName, Guid[] userIds) :
        base(roomId, roomName, userIds)
    {
    }
}
