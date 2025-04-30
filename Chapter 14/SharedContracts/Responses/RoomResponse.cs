using SharedContracts.Events;
using System.Text.Json.Serialization;

namespace SharedContracts.Responses;

public class RoomResponse
{
    [JsonConstructor]
    public RoomResponse(Guid roomId, string roomName, List<UserInfo> roomUsers, List<ChatMessageReceived> messages)
    {
        RoomId = roomId;
        RoomName = roomName;
        RoomUsers = [.. roomUsers];
        Messages = [.. messages];
    }

    public Guid RoomId { get; }
    public string RoomName { get; }

    public List<UserInfo> RoomUsers { get; }

    public List<ChatMessageReceived> Messages { get; }
}
