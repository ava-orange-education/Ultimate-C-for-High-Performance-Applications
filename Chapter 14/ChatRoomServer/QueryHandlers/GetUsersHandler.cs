using ChatRoomServer.Interfaces;
using ChatRoomServer.Queries;
using MediatR;
using SharedContracts;

namespace ChatRoomServer.QueryHandlers;

public class GetUsersHandler(IUserStore userStore) : IRequestHandler<GetUsersQuery, IEnumerable<UserInfo>>
{
    public Task<IEnumerable<UserInfo>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userStore.GetAllUsers());
    }
}
