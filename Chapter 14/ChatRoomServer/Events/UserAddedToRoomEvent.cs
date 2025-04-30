using MediatR;
using SharedContracts.Events;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class UserAddedToRoomEvent : UserAddedToRoom, INotification
{
    [JsonConstructor]
    public UserAddedToRoomEvent(Guid roomId, Guid senderId, Guid[] addedUserIds) : base(roomId, senderId, addedUserIds)
    {
    }
}
