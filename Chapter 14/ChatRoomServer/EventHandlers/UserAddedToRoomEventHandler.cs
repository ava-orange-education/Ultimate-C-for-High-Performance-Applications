using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class UserAddedToRoomEventHandler(IChatRoomStore chatRoomStore, IUserStore userStore) : INotificationHandler<UserAddedToRoomEvent>
{
    public async Task Handle(UserAddedToRoomEvent notification, CancellationToken cancellationToken)
    {
        foreach (var userId in notification.AddedUserIds)
        {
            if (!userStore.CheckUserExists(userId))
            {
                throw new InvalidOperationException($"User {userId} not found.");
            }
        }

        foreach (var userId in notification.AddedUserIds)
        {
            await chatRoomStore.AddUserAsync(notification.RoomId, notification.SenderId, userId);
        }
    }
}
