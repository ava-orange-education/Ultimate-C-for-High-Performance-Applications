using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using System.Collections.Concurrent;

namespace ChatRoomServer.Services;

public class ChatRoomStore(IUserSocketService socketService, IUserStore userStore) : IChatRoomStore
{
    private readonly ConcurrentDictionary<Guid, ChatRoom> chatRooms = new();

    public void CreateRoom(Guid roomId, string roomName, IEnumerable<Guid> userIds)
    {
        var chatRoom = new ChatRoom(socketService, roomName);
        chatRoom.CreateRoom(roomId, userIds);
        chatRooms.TryAdd(roomId, chatRoom);
    }

    public async Task AddUserAsync(Guid roomId, Guid senderId, Guid userId)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            var userInfo = userStore.GetUser(userId) ?? throw new ArgumentException($"User with ID {userId} does not exist.");
            await chatRoom.AddUserAsync(senderId, userInfo);
        }
    }

    public async Task RemoveUserAsync(Guid roomId, Guid userId, Guid senderId)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            if (await chatRoom.RemoveUserAsync(userId, senderId))
            {
                chatRooms.TryRemove(roomId, out _);
            }
        }
    }

    public IEnumerable<ChatRoom> GetAllRooms()
    {
        return chatRooms.Values;
    }

    public IEnumerable<ChatRoom> GetRoomsForUser(Guid userId)
    {
        return chatRooms.Values.Where(room => room.ContainsUser(userId));
    }

    public async Task ReceiveMessageAsync(ChatMessageReceivedEvent message)
    {
        if (chatRooms.TryGetValue(message.RoomId, out var chatRoom))
        {
            await chatRoom.ReceiveMessageAsync(message);
        }
    }
}
