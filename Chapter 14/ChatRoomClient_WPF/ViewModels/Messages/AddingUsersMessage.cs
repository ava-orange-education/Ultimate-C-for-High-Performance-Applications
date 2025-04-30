namespace ChatRoomClient.ViewModels.Messages;

public class AddingUsersMessage(Guid roomId, Guid currentUserId)
{
    public Guid RoomId { get; } = roomId;

    public Guid CurrentUserId { get; } = currentUserId;
}
