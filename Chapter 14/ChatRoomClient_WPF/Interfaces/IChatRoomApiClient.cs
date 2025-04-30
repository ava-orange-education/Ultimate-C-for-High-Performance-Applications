using SharedContracts;
using SharedContracts.Events;
using SharedContracts.Responses;

namespace ChatRoomClient.Interfaces;
public interface IChatRoomApiClient
{
    Task AddUserToRoomAsync(UserAddedToRoom userAddedToRoom, CancellationToken cancellationToken);
    Task CreateRoomAsync(RoomCreated roomCreated, CancellationToken cancellationToken);
    Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<IEnumerable<RoomResponse>> GetRoomsForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserInfo> LoginUserAsync(string userName, CancellationToken cancellationToken);
    Task RemoveUserFromRoomAsync(UserRemovedFromRoom userRemovedFromRoom, CancellationToken cancellationToken);
}