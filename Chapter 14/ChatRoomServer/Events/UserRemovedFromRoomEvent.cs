using MediatR;
using SharedContracts.Events;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class UserRemovedFromRoomEvent : UserRemovedFromRoom, INotification
{
    [JsonConstructor]
    public UserRemovedFromRoomEvent(Guid roomId, Guid userId, Guid senderId) : base(roomId, userId, senderId)
    {
    }
}
