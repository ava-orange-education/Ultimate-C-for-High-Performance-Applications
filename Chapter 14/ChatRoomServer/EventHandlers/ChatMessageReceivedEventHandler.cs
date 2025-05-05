using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Events;

namespace ChatRoomServer.EventHandlers;

public class ChatMessageReceivedEventHandler(IChatRoomStore chatRoomStore, IBroadcastService broadcastService) : INotificationHandler<ChatMessageReceivedEvent>
{
    public async Task Handle(ChatMessageReceivedEvent notification, CancellationToken cancellationToken)
    {
        var roomUsers = chatRoomStore.GetRoomUsers(notification.RoomId, notification.User.UserId);
        await broadcastService.BroadcastReceivedMessageAsync(roomUsers, notification);
    }
}
