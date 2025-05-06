using ChatRoomServer.ErrorHandlers;
using ChatRoomServer.Interfaces;
using SharedContracts;
using SharedContracts.Commands;
using System.Collections.Concurrent;

namespace ChatRoomServer.Services;

public class ChatRoomStore : IChatRoomStore
{
    private readonly ConcurrentDictionary<Guid, ChatRoom> chatRooms = new();

    public void CreateRoom(Guid roomId, string roomName, IEnumerable<Guid> userIds)
    {
        var chatRoom = new ChatRoom(roomName);
        chatRoom.CreateRoom(roomId, userIds);
        chatRooms.TryAdd(roomId, chatRoom);
    }

    public void AddUser(Guid roomId, Guid userIdToAdd)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            chatRoom.AddUser(userIdToAdd);
        }
        else
        {
            throw new StatusCodeException(ErrorType.RoomIdNotFound, roomId.ToString());
        }
    }

    public bool RemoveUser(Guid roomId, Guid userId, Guid senderId)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            if (chatRoom.RemoveUser(userId, senderId))
            {
                chatRooms.TryRemove(roomId, out _);
                return false;
            }
            return true;
        }
        throw new StatusCodeException(ErrorType.RoomIdNotFound, roomId.ToString());
    }

    public IEnumerable<ChatRoom> GetAllRooms()
    {
        return chatRooms.Values;
    }

    public IEnumerable<ChatRoom> GetRoomsForUser(Guid userId)
    {
        return chatRooms.Values.Where(room => room.ContainsUser(userId));
    }

    public IEnumerable<ChatMessage> GetRoomMessages(Guid roomId)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            return chatRoom.GetMessages();
        }
        throw new StatusCodeException(ErrorType.RoomIdNotFound, roomId.ToString());
    }

    public IEnumerable<Guid> GetRoomUsers(Guid roomId, Guid exceptUserId)
    {
        if (chatRooms.TryGetValue(roomId, out var chatRoom))
        {
            return chatRoom.GetUserIds(exceptUserId);
        }
        throw new StatusCodeException(ErrorType.RoomIdNotFound, roomId.ToString());
    }

    public bool StoreMessage(SendChatMessageCommand message)
    {
        if (chatRooms.TryGetValue(message.RoomId, out var chatRoom))
        {
            return chatRoom.StoreMessage(message);
        }
        throw new StatusCodeException(ErrorType.RoomIdNotFound, message.RoomId.ToString());
    }
}
