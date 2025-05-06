using SharedContracts.Commands;

namespace ChatRoomClient.Interfaces;
public interface IWebSocketsClient
{
    bool IsOpen { get; }

    Task ConnectAsync(Guid userId, CancellationToken cancellationToken);
    Task DisconnectAsync(CancellationToken cancellationToken);
    Task SendMessageAsync(SendChatMessageCommand message, CancellationToken cancellationToken);
}