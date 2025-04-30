using SharedContracts;
using SharedContracts.Events;
using System.Collections.Concurrent;

namespace ChatRoomClient.Models;
public class ChatRoom(string name, Guid id)
{
    private readonly ConcurrentDictionary<Guid, ChatMessageReceived> messages = new();
    private readonly ConcurrentDictionary<Guid, UserInfo> users = new();

    public string Name { get; } = name;

    public Guid Id { get; } = id;

    public void AddUser(UserInfo user)
    {
        ArgumentNullException.ThrowIfNull(user);
        users.TryAdd(user.UserId, user);
    }

    public bool ContainsMessage(Guid messageId)
    {
        return messages.ContainsKey(messageId);
    }

    public void RemoveUser(Guid userId)
    {
        users.TryRemove(userId, out _);
    }

    public void AddMessage(ChatMessageReceived message)
    {
        ArgumentNullException.ThrowIfNull(message);
        messages.TryAdd(message.Id, message);
    }

    public IEnumerable<ChatMessageReceived> GetMessages()
    {
        return messages.Values;
    }

    public IEnumerable<UserInfo> GetUsers()
    {
        return users.Values;
    }
}
