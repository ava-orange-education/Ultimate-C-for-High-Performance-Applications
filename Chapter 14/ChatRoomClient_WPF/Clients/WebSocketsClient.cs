using ChatRoomClient.Configuration;
using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedContracts.Commands;
using SharedContracts.Messaging;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ChatRoomClient.Clients;

public class WebSocketsClient(ILogger<WebSocketsClient> logger,
    IOptions<ServerConfig> options,
    IMessenger messenger) : IDisposable, IWebSocketsClient
{
    private readonly ClientWebSocket webSocket = new();
    private readonly CancellationTokenSource readerCts = new();
    private readonly Channel<string> channel = Channel.CreateUnbounded<string>();
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
    private Task receiveTask = Task.CompletedTask;
    private Task writeTask = Task.CompletedTask;

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
        writeTask = WriteMessagesAsync(cancellationToken);
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
            channel.Writer.Complete(); // Complete the channel writer
            await writeTask; // Wait for the write task to complete
        }
    }

    public async Task SendMessageAsync(SendChatMessageCommand message, CancellationToken cancellationToken)
    {
        if (IsOpen)
        {
            if (channel.Reader.Completion.IsCompleted)
            {
                throw new InvalidOperationException("Channel is completed. Cannot send messages.");
            }

            var json = JsonSerializer.Serialize<MessageBase>(message, jsonSerializerOptions);
            logger.LogDebug("Sending message: {Message}", json);
            if (await channel.Writer.WaitToWriteAsync(cancellationToken))
            {
                await channel.Writer.WriteAsync(json, cancellationToken);
            }
        }
        else
        {
            logger.LogWarning("WebSocket is not open. Cannot send message.");
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

                    var messageObj = JsonSerializer.Deserialize<MessageBase>(message, jsonSerializerOptions) ??
                        throw new JsonException("Failed to deserialize message.");
                    logger.LogDebug("Deserialized message type: {Message}", messageObj.GetType());
                    messenger.Publish(messageObj);
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

    private async Task WriteMessagesAsync(CancellationToken cancellationToken)
    {
        await foreach (var message in channel.Reader.ReadAllAsync(cancellationToken))
        {
            logger.LogDebug("Sending message: {Message}", message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken);
            }
        }
    }
}
