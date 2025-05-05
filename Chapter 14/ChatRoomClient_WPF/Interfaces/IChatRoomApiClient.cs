using SharedContracts;
using SharedContracts.Commands;
using SharedContracts.Responses;

namespace ChatRoomClient.Interfaces;
public interface IChatRoomApiClient
{
    Task AddUserToRoomAsync(AddUserToRoomCommand userAddedToRoom, CancellationToken cancellationToken);
    Task CreateRoomAsync(CreateRoomCommand roomCreated, CancellationToken cancellationToken);
    Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<IEnumerable<RoomResponse>> GetRoomsForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserInfo> LoginUserAsync(string userName, CancellationToken cancellationToken);
    Task RemoveUserFromRoomAsync(RemoveUserFromRoomCommand userRemovedFromRoom, CancellationToken cancellationToken);
}