using SharedContracts;

namespace ChatRoomClient.ViewModels.Messages;
public class RoomUsersAddedMessage(Guid roomId, IEnumerable<UserInfo> users)
{
    public Guid RoomId { get; } = roomId;

    public IEnumerable<UserInfo> Users { get; } = users;
}
