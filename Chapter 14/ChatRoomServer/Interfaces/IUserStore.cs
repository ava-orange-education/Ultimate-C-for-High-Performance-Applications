using SharedContracts;

namespace ChatRoomServer.Interfaces;

public interface IUserStore
{
    bool CheckUserExists(Guid userId);
    IEnumerable<UserInfo> GetAllUsers();
    UserInfo? GetUser(Guid userId);
    UserInfo LoginUser(string userName);
}