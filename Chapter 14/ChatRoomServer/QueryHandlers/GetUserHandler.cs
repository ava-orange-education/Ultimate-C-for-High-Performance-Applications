using ChatRoomServer.Interfaces;
using ChatRoomServer.Queries;
using MediatR;
using SharedContracts;

namespace ChatRoomServer.QueryHandlers;

public class GetUserHandler(IUserStore userStore) : IRequestHandler<GetUserQuery, UserInfo?>
{

    public Task<UserInfo?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = userStore.GetUser(request.UserId);
        return Task.FromResult(user);
    }
}
