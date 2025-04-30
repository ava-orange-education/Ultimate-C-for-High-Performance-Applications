using SharedContracts.Events;

namespace ChatRoomClient.ViewModels;
public class UserMessageViewModel(bool IsOwnMessage, string? userName, ChatMessageReceived message)
{
    public bool IsOwnMessage { get; } = IsOwnMessage;
    public string? UserName => userName;

    public Guid UserId => message.User.UserId;

    public Guid MessageId => message.Id;

    public DateTimeOffset Timestamp => message.Timestamp;

    public string Message => message.Message;
}
