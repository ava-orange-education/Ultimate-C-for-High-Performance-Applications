using SharedContracts.Enums;
using System.Net.WebSockets;

namespace ChatRoomServer.Interfaces;

public interface IUserSocketService
{
    void AddSocket(Guid userId, WebSocket webSocket);

    Task ReceiveMessagesAsync(Guid userId);

    Task QueueMessageAsync<T>(Guid userId, MessageType messageType, T message);

    Task CloseAllSockets();
}
