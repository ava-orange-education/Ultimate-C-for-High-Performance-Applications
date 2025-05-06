using MediatR;
using SharedContracts.Enums;
using SharedContracts.Messaging;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ChatRoomServer.Services;

internal class UserSocket(ILogger<UserSocketService> logger, WebSocket webSocket, IMediator mediator) : IDisposable
{
    private Task sendTask = Task.CompletedTask;
    private readonly Channel<string> channel = Channel.CreateUnbounded<string>();
    private readonly CancellationTokenSource readerCts = new();
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public WebSocket WebSocket => webSocket;

    public void Dispose()
    {
        readerCts.Dispose();
        webSocket.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task QueueMessageAsync(MessageType messageType, MessageBase message)
    {
        if (webSocket.State != WebSocketState.Open)
        {
            throw new InvalidOperationException("WebSocket is not open.");
        }

        var jsonString = JsonSerializer.Serialize(message, jsonSerializerOptions);

        if (await channel.Writer.WaitToWriteAsync())
        {
            if (!channel.Writer.TryWrite(jsonString))
            {
                throw new InvalidOperationException("Failed to queue message.");
            }
        }
    }

    public void StartSending()
    {
        sendTask = SendMessagesAsync(CancellationToken.None);
    }

    public async Task StopSendingAsync()
    {
        channel.Writer.Complete();
        await sendTask;
    }

    public async Task ReadMessagesAsync()
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result;
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), readerCts.Token);
                await ProcessReceivedMessageAsync(buffer, result);
            }
            catch (OperationCanceledException)
            {
                logger.LogDebug("Socket canceled: {message}", "Operation canceled");
                await CloseSocketConnectionAsync();
            }
            catch (WebSocketException ex)
            {
                if (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    logger.LogWarning("WebSocket connection closed prematurely: {message}", ex.Message);
                    await CloseSocketConnectionAsync();
                }
                else
                {
                    logger.LogError(ex, "An error occurred while processing messages: {message}", ex.Message);
                    await CloseSocketConnectionAsync();
                }
            }
        }
    }

    private async Task ProcessReceivedMessageAsync(byte[] buffer, WebSocketReceiveResult result)
    {
        if (result.MessageType == WebSocketMessageType.Close)
        {
            logger.LogDebug("WebSocket connection closed by client: {message}", result.CloseStatusDescription);
            await CloseSocketConnectionAsync();
        }
        else if (result.MessageType == WebSocketMessageType.Text)
        {
            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            logger.LogDebug("Received message: {json}", json);

            var message = JsonSerializer.Deserialize<MessageBase>(json, jsonSerializerOptions)
                ?? throw new JsonException("Failed to deserialize message.");

            logger.LogDebug("Publishing message: {message}", message);
            await PublishMessageAsync(message);
        }
    }

    private async Task PublishMessageAsync<T>(T message)
    {
        if (message is INotification)
        {
            await mediator.Publish(
                message,
                readerCts.Token);
        }
        else if (message is IBaseRequest)
        {
            await mediator.Send(
                message,
                readerCts.Token);
        }
        else
        {
            logger.LogWarning("Message is not a valid notification: {message}", message);
        }
    }

    public async Task CloseSocketConnectionAsync()
    {
        if (webSocket.State == WebSocketState.CloseReceived)
        {
            readerCts.Cancel();
            await StopSendingAsync();
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "Closing",
                CancellationToken.None);
            webSocket.Dispose();
        }
    }

    private async Task SendMessagesAsync(CancellationToken cancellationToken)
    {
        await foreach (var message in channel.Reader.ReadAllAsync(cancellationToken))
        {
            logger.LogDebug("Sending message: {message}", message);
            var buffer = Encoding.UTF8.GetBytes(message);
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
