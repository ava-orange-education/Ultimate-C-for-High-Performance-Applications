using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class UserRemovedFromRoomEventHandler(IChatRoomStore chatRoomStore) : INotificationHandler<UserRemovedFromRoomEvent>
{
    public async Task Handle(UserRemovedFromRoomEvent notification, CancellationToken cancellationToken)
    {
        await chatRoomStore.RemoveUserAsync(notification.RoomId, notification.UserId, notification.SenderId);
    }
}
