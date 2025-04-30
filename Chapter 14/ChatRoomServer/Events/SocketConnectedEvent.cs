using MediatR;
using System.Net.WebSockets;

namespace ChatRoomServer.Events;

public class SocketConnectedEvent(Guid userId, WebSocket webSocket) : INotification
{
    public Guid UserId { get; } = userId;

    public WebSocket WebSocket { get; } = webSocket;
}
