using ChatRoomClient.Configuration;
using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedContracts.Enums;
using SharedContracts.Events;
using SharedContracts.Messaging;
using SharedContracts.Notifications;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatRoomClient.Clients;

public class WebSocketsClient(ILogger<WebSocketsClient> logger,
    IOptions<ServerConfig> options,
    IMessenger messenger) : IDisposable, IWebSocketsClient
{
    private readonly ClientWebSocket webSocket = new();
    private readonly CancellationTokenSource readerCts = new();
    private Task receiveTask = Task.CompletedTask;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public bool IsOpen => webSocket.State == WebSocketState.Open;

    public async Task ConnectAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (IsOpen)
        {
            throw new InvalidOperationException("WebSocket is already connected.");
        }

        var uri = options.Value.ToUri();
        var scheme = uri.Scheme == Uri.UriSchemeHttps ? Uri.UriSchemeWss : Uri.UriSchemeWs;
        var uriBuilder = new UriBuilder(uri)
        {
            Path = "/ws",
            Scheme = scheme,
            Query = $"userId={userId}"
        };

        await webSocket.ConnectAsync(uriBuilder.Uri, cancellationToken);

        receiveTask = ReceiveMessagesAsync(readerCts.Token);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        if (IsOpen || webSocket.State == WebSocketState.CloseReceived)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "Closing",
                cancellationToken); // Close the connection gracefully
            readerCts.Cancel(); // Cancel the receive task
            await receiveTask; // Wait for the receive task to complete 
        }
    }

    public async Task SendMessageAsync(ChatMessageReceivedEvent message, CancellationToken cancellationToken)
    {
        if (IsOpen)
        {
            var envelope = new Envelope<ChatMessageReceivedEvent>(MessageType.ChatMessage, message);
            var json = JsonSerializer.Serialize(envelope, jsonSerializerOptions);
            logger.LogDebug("Sending message: {Message}", json);

            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken);
        }
    }

    public void Dispose()
    {
        webSocket.Dispose();
        readerCts.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await DisconnectAsync(CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    logger.LogDebug("Received message: {Message}", message);
                    var messageObj = DeserializeMessage(message);
                    PublishMessage(messageObj);
                }
                else
                {
                    logger.LogWarning("Received unsupported message type: {MessageType}", result.MessageType);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogDebug("Receive operation canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error receiving message: {Message}", ex.Message);
            }
        }
    }

    private object? DeserializeMessage(string jsonString)
    {
        var document = JsonDocument.Parse(jsonString);
        if (document.RootElement.TryGetProperty("messageType", out var messageType))
        {
            if (Enum.TryParse(messageType.GetString()!, true, out MessageType messageTypeEnum))
            {
                if (document.RootElement.TryGetProperty("message", out var message))
                {
                    logger.LogDebug("Deserializing message of type: {MessageType}", messageTypeEnum);
                    return message.Deserialize(GetTypeFromMessageType(messageTypeEnum), jsonSerializerOptions);
                }
                else
                {
                    logger.LogError("Message property not found in JSON: {jsonString}", jsonString);
                }
            }
            else
            {
                logger.LogError("Failed to parse message type: {MessageType}", messageType.GetString());
            }
        }
        else
        {
            logger.LogError("MessageType not found in JSON: {jsonString}", jsonString);
        }
        return default;
    }

    private static Type GetTypeFromMessageType(MessageType messageType)
    {
        return messageType switch
        {
            MessageType.ChatMessage => typeof(ChatMessageReceivedEvent),
            MessageType.UserJoinedChatNotification => typeof(UserJoinedChatNotification),
            MessageType.UserLeftChatNotification => typeof(UserLeftChatNotification),
            _ => throw new ArgumentOutOfRangeException(nameof(messageType), $"Unhandled message type: {messageType}")
        };
    }

    private void PublishMessage(object? message)
    {
        if (message == null)
        {
            logger.LogError("Received null message.");
            return;
        }

        switch (message)
        {
            case ChatMessageReceivedEvent chatMessage:
                messenger.Publish(chatMessage);
                break;
            case UserJoinedChatNotification userJoined:
                messenger.Publish(userJoined);
                break;
            case UserLeftChatNotification userLeft:
                messenger.Publish(userLeft);
                break;
        }
    }
}
