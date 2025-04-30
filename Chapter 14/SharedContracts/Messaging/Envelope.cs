using SharedContracts.Enums;

namespace SharedContracts.Messaging;

public class Envelope<T>(MessageType messageType, T message)
{
    public MessageType MessageType { get; } = messageType;
    public T Message { get; } = message;
}
