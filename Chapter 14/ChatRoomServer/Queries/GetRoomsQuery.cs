using MediatR;
using SharedContracts.Responses;

namespace ChatRoomServer.Queries;

public class GetRoomsQuery(Guid userId) : IRequest<IEnumerable<RoomResponse>>
{
    public Guid UserId { get; } = userId;
}
