using SharedContracts.Enums;
using SharedContracts.Messaging;
using System.Net.WebSockets;

namespace ChatRoomServer.Interfaces;

public interface IUserSocketService
{
    void AddSocket(Guid userId, WebSocket webSocket);

    Task ReceiveMessagesAsync(Guid userId);

    Task QueueMessageAsync(Guid userId, MessageType messageType, MessageBase message);

    Task CloseAllSockets();
}
