using ChatRoomServer.Interfaces;
using SharedContracts;
using System.Collections.Concurrent;

namespace ChatRoomServer.Services;

public class UserStore : IUserStore
{
    private readonly ConcurrentDictionary<Guid, UserInfo> users = new();

    public bool CheckUserExists(Guid userId)
    {
        return users.ContainsKey(userId);
    }

    public UserInfo? GetUser(Guid userId)
    {
        if (users.TryGetValue(userId, out var userInfo))
        {
            return userInfo;
        }
        return null;
    }

    public IEnumerable<UserInfo> GetAllUsers()
    {
        return users.Values;
    }

    public UserInfo LoginUser(string userName)
    {
        var userInfo = users.Values.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        if (userInfo != null)
        {
            return userInfo;
        }

        var newUserId = Guid.NewGuid();
        var newUserInfo = new UserInfo(newUserId, userName);
        users.TryAdd(newUserId, newUserInfo);
        return newUserInfo;
    }
}
