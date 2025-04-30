using ChatRoomServer.Interfaces;
using ChatRoomServer.Queries;
using MediatR;
using SharedContracts;

namespace ChatRoomServer.QueryHandlers;

public class LoginUserQueryHandler(IUserStore userStore) : IRequestHandler<LoginUserQuery, UserInfo>
{
    public Task<UserInfo> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userStore.LoginUser(request.UserName));
    }
}
