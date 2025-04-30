using System.Text.Json.Serialization;

namespace SharedContracts.Events;

public class RoomCreated
{
    [JsonConstructor]
    public RoomCreated(Guid roomId, string roomName, Guid[] userIds)
    {
        RoomId = roomId;
        RoomName = roomName;
        UserIds = userIds;
    }

    public Guid RoomId { get; }
    public string RoomName { get; }

    public Guid[] UserIds { get; }
}
