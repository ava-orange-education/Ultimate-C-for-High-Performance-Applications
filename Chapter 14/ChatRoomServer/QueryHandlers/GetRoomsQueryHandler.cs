using ChatRoomServer.Interfaces;
using ChatRoomServer.Queries;
using MediatR;
using SharedContracts.Events;
using SharedContracts.Responses;

namespace ChatRoomServer.QueryHandlers;

public class GetRoomsQueryHandler(IChatRoomStore chatRoomStore, IUserStore userStore) : IRequestHandler<GetRoomsQuery, IEnumerable<RoomResponse>>
{
    public Task<IEnumerable<RoomResponse>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<RoomResponse>();
        foreach (var room in chatRoomStore.GetRoomsForUser(request.UserId))
        {
            var roomUsers = room.GetUserIds(null).Select(userStore.GetUser).ToList();
            var messages = room.GetMessages().Select(m => new ChatMessageReceived(m.User, m.RoomId, m.Id, m.Timestamp, m.Message)).ToList();
            result.Add(new RoomResponse(room.RoomId, room.RoomName, roomUsers!, messages));
        }
        return Task.FromResult(result.AsEnumerable());
    }
}
