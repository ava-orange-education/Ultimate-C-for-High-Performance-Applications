using SharedContracts;
using SharedContracts.Events;

namespace ChatRoomServer.Interfaces;
public interface IBroadcastService
{
    Task BroadcastReceivedMessageAsync(IEnumerable<Guid> userIds, ChatMessageReceivedEvent message);
    Task BroadcastUserJoinedChatNotificationAsync(Guid roomId, UserInfo addedUser, IEnumerable<Guid> userIds);
    Task BroadcastUserLeftChatNotificationAsync(Guid roomId, Guid removedUserId, IEnumerable<Guid> existingUserIds);
    Task QueueMessageHistoryAsync(Guid userId, IEnumerable<ChatMessageReceivedEvent> chatMessages);
}