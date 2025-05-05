using ChatRoomServer.Services;
using SharedContracts;
using SharedContracts.Commands;

namespace ChatRoomServer.Interfaces;

public interface IChatRoomStore
{
    void AddUser(Guid roomId, Guid userIdToAdd);
    void CreateRoom(Guid roomId, string roomName, IEnumerable<Guid> userIds);
    IEnumerable<ChatRoom> GetAllRooms();
    IEnumerable<ChatMessage> GetRoomMessages(Guid roomId);
    IEnumerable<ChatRoom> GetRoomsForUser(Guid userId);
    IEnumerable<Guid> GetRoomUsers(Guid roomId, Guid exceptUserId);
    bool RemoveUser(Guid roomId, Guid userId, Guid senderId);
    bool StoreMessage(SendChatMessageCommand message);
}