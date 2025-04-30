using System.Text.Json.Serialization;

namespace SharedContracts.Events;

public class UserRemovedFromRoom
{
    [JsonConstructor]
    public UserRemovedFromRoom(Guid roomId, Guid userId, Guid senderId)
    {
        RoomId = roomId;
        UserId = userId;
        SenderId = senderId;
    }

    public Guid RoomId { get; }
    public Guid UserId { get; }

    public Guid SenderId { get; }
}
