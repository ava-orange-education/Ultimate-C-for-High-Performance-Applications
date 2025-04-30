using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using SharedContracts;
using SharedContracts.Enums;
using SharedContracts.Notifications;
using System.Collections.Concurrent;

namespace ChatRoomServer.Services;

public class ChatRoom(IUserSocketService socketStore, string roomName)
{
    private Guid roomId;
    private readonly ConcurrentDictionary<Guid, byte> userIds = new();
    private readonly ConcurrentDictionary<Guid, ChatMessageReceivedEvent> messages = new();

    public Guid RoomId => roomId;

    public string RoomName => roomName;

    public bool ContainsUser(Guid userId) => userIds.ContainsKey(userId);

    public IEnumerable<ChatMessageReceivedEvent> GetMessages() => messages.Values;

    public IEnumerable<Guid> GetUserIds(Guid? exceptUser)
    {
        if (exceptUser == null)
        {
            return userIds.Keys;
        }

        return userIds.Keys.Where(uid => uid != exceptUser);
    }

    public void CreateRoom(Guid roomId, IEnumerable<Guid> userIds)
    {
        this.roomId = roomId;
        foreach (var userId in userIds)
        {
            this.userIds.TryAdd(userId, 0);
        }
    }

    public async Task AddUserAsync(Guid senderId, UserInfo userInfo)
    {
        if (userIds.TryAdd(userInfo.UserId, 0))
        {
            var notification = new UserJoinedChatNotification(roomId, userInfo);
            foreach (var existingUser in userIds.Keys.Where(uid => uid != senderId))
            {
                await socketStore.QueueMessageAsync(existingUser,
                    MessageType.UserJoinedChatNotification,
                    notification);
            }

            foreach (var message in messages)
            {
                await socketStore.QueueMessageAsync(userInfo.UserId,
                    MessageType.ChatMessage,
                    message.Value);
            }
        }
    }

    public async Task<bool> RemoveUserAsync(Guid userId, Guid senderId)
    {
        if (userIds.TryRemove(userId, out _))
        {
            var notification = new UserLeftChatNotification(roomId, userId);
            foreach (var existingUser in userIds.Keys.Where(uid => uid != senderId))
            {
                await socketStore.QueueMessageAsync(existingUser,
                    MessageType.UserLeftChatNotification,
                    notification);
            }

            await socketStore.QueueMessageAsync(userId,
                MessageType.UserLeftChatNotification,
                notification);
        }

        return userIds.IsEmpty;
    }

    public async Task ReceiveMessageAsync(ChatMessageReceivedEvent message)
    {
        if (messages.TryAdd(message.Id, message))
        {
            foreach (var userId in userIds.Keys.Where(uid => message.User.UserId != uid))
            {
                await socketStore.QueueMessageAsync(userId, MessageType.ChatMessage, message);
            }
        }
    }
}
