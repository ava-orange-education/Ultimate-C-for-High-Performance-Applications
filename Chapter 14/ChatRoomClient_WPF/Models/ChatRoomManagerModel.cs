using ChatRoomClient.Interfaces;
using Microsoft.Extensions.Logging;
using SharedContracts;
using SharedContracts.Commands;
using SharedContracts.Events;
using SharedContracts.Responses;
using System.Collections.Concurrent;

namespace ChatRoomClient.Models;

public class ChatRoomManagerModel(ILogger<ChatRoomManagerModel> logger,
    IChatRoomApiClient chatRoomApiClient,
    IWebSocketsClient webSocketsClient) : IChatRoomManagerModel
{
    private readonly ConcurrentDictionary<Guid, ChatRoom> chatRooms = new();
    private readonly List<UserInfo> emptyUsers = [];
    private readonly List<ChatMessageReceivedEvent> emptyMessages = [];

    public UserInfo? ChatUser { get; private set; }

    public Guid? SelectedChatRoomId { get; set; }

    public async Task LoginAsync(string userName)
    {
        logger.LogDebug("LOGIN {userName}", userName);
        var userInfo = await chatRoomApiClient.LoginUserAsync(userName, CancellationToken.None);
        await webSocketsClient.ConnectAsync(userInfo.UserId, CancellationToken.None);
        ChatUser = userInfo;
    }

    public async Task LogoutAsync()
    {
        await webSocketsClient.DisconnectAsync(CancellationToken.None);
        logger.LogDebug("LOGOUT");
    }

    public async Task<IEnumerable<RoomResponse>> GetRoomsForUserAsync()
    {
        var userId = ChatUser?.UserId ?? Guid.Empty;
        logger.LogDebug("GET ROOMS FOR USER {userId}", userId);

        var rooms = await chatRoomApiClient.GetRoomsForUserAsync(userId, CancellationToken.None);
        foreach (var room in rooms)
        {
            var chatRoom = new ChatRoom(room.RoomName, room.RoomId);
            if (chatRooms.TryAdd(room.RoomId, chatRoom))
            {
                foreach (var user in room.RoomUsers)
                {
                    chatRoom.AddUser(user);
                }
                foreach (var message in room.Messages)
                {
                    chatRoom.AddMessage(message);
                }
            }
        }
        return rooms;
    }

    public async Task AddChatRoomAsync(string name, Guid id, bool localAdd = true)
    {
        logger.LogDebug("ADD CHAT ROOM {name} {id}", name, id);

        var room = new ChatRoom(name, id);
        if (localAdd)
        {
            var roomCreated = new CreateRoomCommand(id, name, [ChatUser!.UserId]);
            await chatRoomApiClient.CreateRoomAsync(roomCreated, CancellationToken.None);
            room.AddUser(ChatUser!);
        }
        chatRooms.TryAdd(id, room);
    }

    public void RemoveChatRoom(Guid id)
    {
        logger.LogDebug("REMOVE CHAT ROOM {id}", id);

        chatRooms.TryRemove(id, out _);
    }

    public async Task<ChatMessageReceivedEvent?> AddMessageAsync(Guid roomId, string message)
    {
        var user = ChatUser!;

        logger.LogDebug("ADD MESSAGE {roomId} {userId} {message}", roomId, user, message);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            var sendChatMessageCommand = new SendChatMessageCommand(user,
                roomId,
                Guid.NewGuid(),
                DateTimeOffset.Now,
                message);

            await webSocketsClient.SendMessageAsync(sendChatMessageCommand, CancellationToken.None);

            var chatMessageReceived = new ChatMessageReceivedEvent(sendChatMessageCommand.User,
                sendChatMessageCommand.RoomId,
                sendChatMessageCommand.Id,
                sendChatMessageCommand.Timestamp,
                sendChatMessageCommand.Message);

            room.AddMessage(chatMessageReceived);
            return chatMessageReceived;
        }
        return null;
    }

    public void ReceiveMessage(ChatMessageReceivedEvent message)
    {
        logger.LogDebug("RECEIVE MESSAGE {message}", message);

        if (chatRooms.TryGetValue(message.RoomId, out var room))
        {
            if (!room.ContainsMessage(message.Id))
            {
                room.AddMessage(message);
            }
        }
    }

    public async Task AddUsersAsync(Guid roomId, IEnumerable<UserInfo> users)
    {
        logger.LogDebug("ADD USERS {roomId}", roomId);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            var userAddedToRoom = new AddUserToRoomCommand(roomId, ChatUser!.UserId, [.. users.Select(u => u.UserId)]);

            await chatRoomApiClient.AddUserToRoomAsync(userAddedToRoom, CancellationToken.None);

            foreach (var user in users)
            {
                room.AddUser(user);
            }
        }
    }

    public void AddUser(Guid roomId, UserInfo user)
    {
        logger.LogDebug("ADD USER {roomId} {user}", roomId, user.UserName);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            room.AddUser(user);
        }
    }

    public async Task RemoveUserAsync(Guid roomId, Guid userId, bool addedLocally = true)
    {
        logger.LogDebug("REMOVE USER {roomId} {userId}", roomId, userId);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            if (addedLocally)
            {
                var userRemovedFromRoom = new RemoveUserFromRoomCommand(roomId, userId, ChatUser!.UserId);
                await chatRoomApiClient.RemoveUserFromRoomAsync(userRemovedFromRoom, CancellationToken.None);
            }
            room.RemoveUser(userId);
        }
    }

    public IEnumerable<UserInfo> GetUsers(Guid roomId)
    {
        logger.LogDebug("GET USERS {roomId}", roomId);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            return room.GetUsers();
        }
        return emptyUsers;
    }

    public IEnumerable<ChatMessageReceivedEvent> GetMessages(Guid roomId)
    {
        logger.LogDebug("GET MESSAGES {roomId}", roomId);

        if (chatRooms.TryGetValue(roomId, out var room))
        {
            return room.GetMessages();
        }
        return emptyMessages;
    }
}
