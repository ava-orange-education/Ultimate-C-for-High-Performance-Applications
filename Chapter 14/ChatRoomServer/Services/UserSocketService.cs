using ChatRoomServer.Interfaces;
using MediatR;
using SharedContracts.Enums;
using SharedContracts.Messaging;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ChatRoomServer.Services;

public class UserSocketService(ILogger<UserSocketService> logger, IMediator mediator) : IUserSocketService
{
    private readonly ConcurrentDictionary<Guid, UserSocket> sockets = new();

    public void AddSocket(Guid userId, WebSocket webSocket)
    {
        logger.LogDebug("Adding socket for user: {userId}", userId);

        if (sockets.ContainsKey(userId))
        {
            throw new InvalidOperationException("User already has an active socket.");
        }
        var userSocket = new UserSocket(logger, webSocket, mediator);
        userSocket.StartSending();
        sockets.TryAdd(userId, userSocket);
    }

    public async Task CloseAllSockets()
    {
        logger.LogDebug("Closing all sockets");

        foreach (var kv in sockets)
        {
            await kv.Value.CloseSocketConnectionAsync();
            kv.Value.Dispose();
        }
    }

    public async Task QueueMessageAsync(Guid userId, MessageType messageType, MessageBase message)
    {
        logger.LogDebug("Queueing message for user: {userId}, messageType: {messageType}", userId, messageType);

        if (sockets.TryGetValue(userId, out var userSocket))
            await userSocket.QueueMessageAsync(messageType, message);
        else
            logger.LogError("User socket not found for user: {userId}", userId);
    }

    public async Task ReceiveMessagesAsync(Guid userId)
    {
        logger.LogDebug("Receiving messages for user: {userId}", userId);

        if (sockets.TryGetValue(userId, out var userSocket))
        {
            await userSocket.ReadMessagesAsync();
            sockets.TryRemove(userId, out _);
            userSocket.Dispose();
            logger.LogDebug("Socket removed for user: {userId}", userId);
        }
        else
        {
            throw new InvalidOperationException("User socket not found.");
        }
    }
}
