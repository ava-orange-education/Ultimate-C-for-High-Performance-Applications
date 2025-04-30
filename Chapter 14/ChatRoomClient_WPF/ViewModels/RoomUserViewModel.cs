using SharedContracts;

namespace ChatRoomClient.ViewModels;

public class RoomUserViewModel(UserInfo user) : ViewModelBase
{
    public string UserName => user.UserName;

    public Guid Id => user.UserId;
}
