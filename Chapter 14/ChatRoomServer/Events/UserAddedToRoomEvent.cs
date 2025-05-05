using MediatR;
using SharedContracts;

namespace ChatRoomServer.Events;

public class UserAddedToRoomEvent : INotification
{
    public UserAddedToRoomEvent(Guid roomId, Guid senderId, UserInfo addedUser)
    {
        RoomId = roomId;
        SenderId = senderId;
        AddedUser = addedUser;
    }

    public Guid RoomId { get; }

    public Guid SenderId { get; }

    public UserInfo AddedUser { get; }
}
