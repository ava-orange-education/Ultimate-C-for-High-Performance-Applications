using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class UserRemovedFromRoomEventHandler(IChatRoomStore chatRoomStore, IBroadcastService broadcastService) : INotificationHandler<UserRemovedFromRoomEvent>
{
    public Task Handle(UserRemovedFromRoomEvent notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        //Notify all users except the sender that the user has left the room
        var existingUserIds = chatRoomStore.GetRoomUsers(notification.RoomId, notification.SenderId);
        return broadcastService.BroadcastUserLeftChatNotificationAsync(notification.RoomId,
            notification.RemovedUserId,
            existingUserIds);
    }
}
