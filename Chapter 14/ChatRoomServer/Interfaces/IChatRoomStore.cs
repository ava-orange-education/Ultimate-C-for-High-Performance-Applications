using ChatRoomServer.Events;
using ChatRoomServer.Services;

namespace ChatRoomServer.Interfaces;

public interface IChatRoomStore
{
    Task AddUserAsync(Guid roomId, Guid senderId, Guid userId);
    void CreateRoom(Guid roomId, string roomName, IEnumerable<Guid> userIds);
    IEnumerable<ChatRoom> GetAllRooms();
    IEnumerable<ChatRoom> GetRoomsForUser(Guid userId);
    Task ReceiveMessageAsync(ChatMessageReceivedEvent message);
    Task RemoveUserAsync(Guid roomId, Guid userId, Guid senderId);
}