
using MediatR;
using System.Text.Json.Serialization;

namespace SharedContracts.Events;

public class ChatMessageReceivedEvent : ChatMessage, INotification
{
    [JsonConstructor]
    public ChatMessageReceivedEvent(UserInfo user,
        Guid roomId,
        Guid id,
        DateTimeOffset timestamp,
        string message) : base(user, roomId, id, timestamp, message)
    {
    }
}
