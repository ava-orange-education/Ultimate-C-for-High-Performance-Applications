using SharedContracts.Events;

namespace ChatRoomClient.Interfaces;
public interface IWebSocketsClient
{
    bool IsOpen { get; }

    Task ConnectAsync(Guid userId, CancellationToken cancellationToken);
    Task DisconnectAsync(CancellationToken cancellationToken);
    Task SendMessageAsync(ChatMessageReceived message, CancellationToken cancellationToken);
}