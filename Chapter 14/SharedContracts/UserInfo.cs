using System.Text.Json.Serialization;

namespace SharedContracts;

public class UserInfo
{
    [JsonConstructor]
    public UserInfo(Guid userId, string userName)
    {
        UserName = userName;
        UserId = userId;
    }

    public string UserName { get; }

    public Guid UserId { get; }
}
