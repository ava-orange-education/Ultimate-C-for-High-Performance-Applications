using MediatR;
using System.Text.Json.Serialization;

namespace SharedContracts.Commands;
public class SendChatMessageCommand : ChatMessage, IRequest<Unit>
{
    [JsonConstructor]
    public SendChatMessageCommand(UserInfo user,
        Guid roomId,
        Guid id,
        DateTimeOffset timestamp,
        string message) : base(user, roomId, id, timestamp, message)
    {
    }
}
