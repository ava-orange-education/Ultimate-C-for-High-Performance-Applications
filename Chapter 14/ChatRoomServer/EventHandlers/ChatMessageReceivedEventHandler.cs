using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class ChatMessageReceivedEventHandler(IChatRoomStore chatRoomStore) : INotificationHandler<ChatMessageReceivedEvent>
{
    public async Task Handle(ChatMessageReceivedEvent notification, CancellationToken cancellationToken)
    {
        await chatRoomStore.ReceiveMessageAsync(notification);
    }
}
