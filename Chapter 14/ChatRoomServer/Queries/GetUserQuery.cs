using MediatR;
using SharedContracts;

namespace ChatRoomServer.Queries;

public class GetUserQuery(Guid id) : IRequest<UserInfo>
{
    public Guid UserId { get; } = id;
}
