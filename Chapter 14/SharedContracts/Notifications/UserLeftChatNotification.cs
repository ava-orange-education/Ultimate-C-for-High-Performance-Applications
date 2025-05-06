using SharedContracts.Messaging;
using System.Text.Json.Serialization;

namespace SharedContracts.Notifications;

public class UserLeftChatNotification : MessageBase
{
    [JsonConstructor]
    public UserLeftChatNotification(Guid roomId, Guid userId)
    {
        RoomId = roomId;
        UserId = userId;
    }

    public Guid RoomId { get; }

    public Guid UserId { get; }
}
