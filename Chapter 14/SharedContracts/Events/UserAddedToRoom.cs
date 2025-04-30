using System.Text.Json.Serialization;

namespace SharedContracts.Events;

public class UserAddedToRoom
{
    [JsonConstructor]
    public UserAddedToRoom(Guid roomId, Guid senderId, Guid[] addedUserIds)
    {
        RoomId = roomId;
        SenderId = senderId;
        AddedUserIds = addedUserIds;
    }

    public Guid RoomId { get; }

    public Guid SenderId { get; }

    public Guid[] AddedUserIds { get; }
}
