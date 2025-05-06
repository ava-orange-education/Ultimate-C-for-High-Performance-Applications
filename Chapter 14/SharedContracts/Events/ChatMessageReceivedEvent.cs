
using MediatR;
using SharedContracts.Commands;
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

    public static ChatMessageReceivedEvent FromCommand(SendChatMessageCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        return new ChatMessageReceivedEvent(
            command.User,
            command.RoomId,
            command.Id,
            command.Timestamp,
            command.Message);
    }
}
