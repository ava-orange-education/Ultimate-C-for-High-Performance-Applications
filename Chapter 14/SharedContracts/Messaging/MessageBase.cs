using SharedContracts.Commands;
using SharedContracts.Enums;
using SharedContracts.Events;
using SharedContracts.Notifications;
using System.Text.Json.Serialization;

namespace SharedContracts.Messaging;

[JsonPolymorphic]
[JsonDerivedType(typeof(SendChatMessageCommand), (int)MessageType.SendChatMessageCommand)]
[JsonDerivedType(typeof(ChatMessageReceivedEvent), (int)MessageType.ChatMessageReceivedEvent)]
[JsonDerivedType(typeof(UserJoinedChatNotification), (int)MessageType.UserJoinedChatNotification)]
[JsonDerivedType(typeof(UserLeftChatNotification), (int)MessageType.UserLeftChatNotification)]
public class MessageBase
{
}
