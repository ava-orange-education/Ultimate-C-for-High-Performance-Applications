using MediatR;
using SharedContracts;

namespace ChatRoomServer.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserInfo>>
{
}
