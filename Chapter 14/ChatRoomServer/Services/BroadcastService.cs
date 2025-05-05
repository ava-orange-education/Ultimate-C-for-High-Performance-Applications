using ChatRoomServer.Interfaces;
using SharedContracts;
using SharedContracts.Enums;
using SharedContracts.Events;
using SharedContracts.Notifications;

namespace ChatRoomServer.Services;

public class BroadcastService(ILogger<BroadcastService> logger, IUserSocketService userSocketService) : IBroadcastService
{
    public async Task BroadcastReceivedMessageAsync(IEnumerable<Guid> userIds, ChatMessageReceivedEvent message)
    {
        ArgumentNullException.ThrowIfNull(userIds, nameof(userIds));
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        logger.LogDebug("Broadcasting message to users: {userIds}", string.Join(", ", userIds));
        foreach (var userId in userIds)
        {
            await userSocketService.QueueMessageAsync(userId, MessageType.ChatMessage, message);
        }
    }

    public async Task BroadcastUserJoinedChatNotificationAsync(Guid roomId, UserInfo addedUser, IEnumerable<Guid> userIds)
    {
        ArgumentNullException.ThrowIfNull(userIds, nameof(userIds));
        ArgumentNullException.ThrowIfNull(addedUser, nameof(addedUser));

        logger.LogDebug("Broadcasting user joined chat to users in room: {roomId}, added user: {addedUser}, Users: {users}",
            roomId, addedUser.UserId, string.Join(", ", userIds));

        //Notify all users except the sender that the new user has joined the room
        var userJoinedChatNotification = new UserJoinedChatNotification(roomId, addedUser);
        foreach (var existingUser in userIds)
        {
            await userSocketService.QueueMessageAsync(existingUser,
                MessageType.UserJoinedChatNotification,
                userJoinedChatNotification);
        }
    }

    public async Task BroadcastUserLeftChatNotificationAsync(Guid roomId, Guid removedUserId, IEnumerable<Guid> existingUserIds)
    {
        //Send a notification to existing users in the room except the sender
        var notification = new UserLeftChatNotification(roomId, removedUserId);
        foreach (var existingUserId in existingUserIds)
        {
            await userSocketService.QueueMessageAsync(existingUserId,
                MessageType.UserLeftChatNotification,
                notification);
        }

        //Ensure the removed user also receives the notification
        await userSocketService.QueueMessageAsync(removedUserId,
            MessageType.UserLeftChatNotification,
            notification);
    }

    public async Task QueueMessageHistoryAsync(Guid userId, IEnumerable<ChatMessageReceivedEvent> chatMessages)
    {
        ArgumentNullException.ThrowIfNull(chatMessages, nameof(chatMessages));

        logger.LogDebug("Queueing message history for user: {userId}", userId);

        foreach (var message in chatMessages)
        {
            await userSocketService.QueueMessageAsync(userId,
                MessageType.ChatMessage,
                message);
        }
    }
}
