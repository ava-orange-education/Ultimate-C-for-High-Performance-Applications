using SharedContracts;
using SharedContracts.Events;
using SharedContracts.Responses;

namespace ChatRoomClient.Interfaces;

public interface IChatRoomManagerModel
{
    UserInfo? ChatUser { get; }

    Guid? SelectedChatRoomId { get; set; }

    Task AddChatRoomAsync(string name, Guid id, bool localAdd = true);
    Task<ChatMessageReceivedEvent?> AddMessageAsync(Guid roomId, string message);
    void AddUser(Guid roomId, UserInfo user);
    Task LoginAsync(string userName);
    Task LogoutAsync();
    void RemoveChatRoom(Guid id);
    void ReceiveMessage(ChatMessageReceivedEvent message);
    Task RemoveUserAsync(Guid roomId, Guid userId, bool addedLocally = true);
    IEnumerable<UserInfo> GetUsers(Guid roomId);
    IEnumerable<ChatMessageReceivedEvent> GetMessages(Guid roomId);
    Task AddUsersAsync(Guid roomId, IEnumerable<UserInfo> users);
    Task<IEnumerable<RoomResponse>> GetRoomsForUserAsync();
}