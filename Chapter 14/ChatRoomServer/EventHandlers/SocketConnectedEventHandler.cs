using ChatRoomServer.Events;
using ChatRoomServer.Interfaces;
using MediatR;

namespace ChatRoomServer.EventHandlers;

public class SocketConnectedEventHandler(ILogger<SocketConnectedEventHandler> logger,
    IUserSocketService socketService) : INotificationHandler<SocketConnectedEvent>
{
    public async Task Handle(SocketConnectedEvent notification, CancellationToken cancellationToken)
    {
        //This cancellationToken is unused in a web socket scenario.
        ArgumentNullException.ThrowIfNull(notification);

        socketService.AddSocket(notification.UserId, notification.WebSocket);
        await socketService.ReceiveMessagesAsync(notification.UserId);

        logger.LogDebug("WebSocket connection closed for user: {userId}.", notification.UserId);
    }
}
