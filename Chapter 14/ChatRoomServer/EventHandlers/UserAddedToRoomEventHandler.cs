using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Events;

namespace ChatRoomServer.EventHandlers;

public class UserAddedToRoomEventHandler(IChatRoomStore chatRoomStore, IBroadcastService broadcastService) : INotificationHandler<UserAddedToRoomEvent>
{
    public async Task Handle(UserAddedToRoomEvent notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        //Notify all users except the sender that the new user has joined the room
        await broadcastService.BroadcastUserJoinedChatNotificationAsync(notification.RoomId,
            notification.AddedUser,
            chatRoomStore.GetRoomUsers(notification.RoomId, notification.SenderId));

        //Send all existing messages to the new user
        await broadcastService.QueueMessageHistoryAsync(notification.AddedUser.UserId,
            chatRoomStore.GetRoomMessages(notification.RoomId)
            .Select(m => new ChatMessageReceivedEvent(m.User,
            m.RoomId,
            m.Id,
            m.Timestamp,
            m.Message)));
    }
}
