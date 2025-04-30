namespace ChatRoomClient.ViewModels.Messages
{
    public class ChatRoomCreatedMessage(string? roomName)
    {
        public string? RoomName { get; } = roomName;
        public Guid RoomId { get; } = Guid.NewGuid();
    }
}
