using MediatR;
using SharedContracts;
using SharedContracts.Events;
using System.Text.Json.Serialization;

namespace ChatRoomServer.Events;

public class ChatMessageReceivedEvent : ChatMessageReceived, INotification
{
    [JsonConstructor]
    public ChatMessageReceivedEvent(UserInfo user, Guid roomId, Guid id, DateTimeOffset timestamp, string message) :
        base(user, roomId, id, timestamp, message)
    {
    }
}
