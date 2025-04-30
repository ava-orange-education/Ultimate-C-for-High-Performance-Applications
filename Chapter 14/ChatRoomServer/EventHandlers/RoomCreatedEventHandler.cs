using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class RoomCreatedEventHandler(IChatRoomStore chatRoomStore) : INotificationHandler<RoomCreatedEvent>
{
    public Task Handle(RoomCreatedEvent notification, CancellationToken cancellationToken)
    {
        chatRoomStore.CreateRoom(notification.RoomId, notification.RoomName, notification.UserIds);
        return Task.CompletedTask;
    }
}
