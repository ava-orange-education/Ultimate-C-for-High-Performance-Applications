using SharedContracts.Messaging;

namespace SharedContracts;

public class ChatMessage(UserInfo user, Guid roomId, Guid id, DateTimeOffset timestamp, string message) : MessageBase
{
    public UserInfo User { get; } = user;

    public Guid RoomId { get; } = roomId;

    public Guid Id { get; } = id;

    public DateTimeOffset Timestamp { get; } = timestamp;

    public string Message { get; } = message;
}
