using MediatR;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class UserRemovedFromRoomEvent : INotification
{
    [JsonConstructor]
    public UserRemovedFromRoomEvent(Guid roomId, Guid userId, Guid senderId)
    {
        RoomId = roomId;
        RemovedUserId = userId;
        SenderId = senderId;
    }

    public Guid RoomId { get; }

    public Guid RemovedUserId { get; }

    public Guid SenderId { get; }

}
