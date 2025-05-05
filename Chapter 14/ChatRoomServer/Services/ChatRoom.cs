using SharedContracts;
using SharedContracts.Commands;
using System.Collections.Concurrent;

namespace ChatRoomServer.Services;

public class ChatRoom(string roomName)
{
    private Guid roomId;
    private readonly ConcurrentDictionary<Guid, byte> userIds = new();
    private readonly ConcurrentDictionary<Guid, ChatMessage> messages = new();

    public Guid RoomId => roomId;

    public string RoomName => roomName;

    public bool ContainsUser(Guid userId) => userIds.ContainsKey(userId);

    public IEnumerable<ChatMessage> GetMessages() => messages.Values;

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

    public bool AddUser(Guid userIdToAdd)
    {
        return userIds.TryAdd(userIdToAdd, 0);
    }

    public bool RemoveUser(Guid userId, Guid senderId)
    {
        _ = userIds.TryRemove(userId, out _);

        return userIds.IsEmpty;
    }

    public bool StoreMessage(SendChatMessageCommand message)
    {
        return messages.TryAdd(message.Id, new ChatMessage(
            message.User,
            message.RoomId,
            message.Id,
            message.Timestamp,
            message.Message));
    }
}
