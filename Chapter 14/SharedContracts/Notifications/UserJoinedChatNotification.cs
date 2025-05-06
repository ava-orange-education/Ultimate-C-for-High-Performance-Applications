using SharedContracts.Messaging;
using System.Text.Json.Serialization;

namespace SharedContracts.Notifications;

public class UserJoinedChatNotification : MessageBase
{
    [JsonConstructor]
    public UserJoinedChatNotification(Guid roomId, Guid userId, string userName)
    {
        RoomId = roomId;
        UserId = userId;
        UserName = userName;
    }

    public UserJoinedChatNotification(Guid roomId, UserInfo userInfo)
    {
        RoomId = roomId;
        UserId = userInfo.UserId;
        UserName = userInfo.UserName;
    }

    public Guid RoomId { get; }

    public Guid UserId { get; }

    public string UserName { get; }
}
