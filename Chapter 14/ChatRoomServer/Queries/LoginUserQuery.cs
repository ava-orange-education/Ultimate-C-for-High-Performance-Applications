using MediatR;
using SharedContracts;

namespace ChatRoomServer.Queries;

public class LoginUserQuery(string userName) : IRequest<UserInfo>
{
    public string UserName { get; } = userName;
}
