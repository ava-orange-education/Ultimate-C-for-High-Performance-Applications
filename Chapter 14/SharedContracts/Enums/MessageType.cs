namespace SharedContracts.Enums;

public enum MessageType
{
    None,
    SendChatMessageCommand,
    ChatMessageReceivedEvent,
    UserJoinedChatNotification,
    UserLeftChatNotification
}
