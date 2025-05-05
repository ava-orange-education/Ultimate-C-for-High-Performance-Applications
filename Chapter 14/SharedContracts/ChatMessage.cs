namespace SharedContracts;
public class ChatMessage
{
    public ChatMessage(UserInfo user, Guid roomId, Guid id, DateTimeOffset timestamp, string message)
    {
        User = user;
        RoomId = roomId;
        Id = id;
        Timestamp = timestamp;
        Message = message;
    }

    public UserInfo User { get; }

    public Guid RoomId { get; }

    public Guid Id { get; }

    public DateTimeOffset Timestamp { get; }

    public string Message { get; }
}
