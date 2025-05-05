using ChatRoomClient.Interfaces;
using SharedContracts;
using SharedContracts.Commands;
using SharedContracts.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;

namespace ChatRoomClient.Clients;
public class ChatRoomApiClient(HttpClient httpClient) : IChatRoomApiClient
{
    public async Task<IEnumerable<RoomResponse>> GetRoomsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"api/rooms?userId={HttpEncode(userId.ToString())}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomResponse>>(cancellationToken: cancellationToken);
        return rooms!;
    }

    public async Task CreateRoomAsync(CreateRoomCommand roomCreated, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("api/rooms", roomCreated, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddUserToRoomAsync(AddUserToRoomCommand userAddedToRoom, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync("api/rooms", userAddedToRoom, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveUserFromRoomAsync(RemoveUserFromRoomCommand userRemovedFromRoom, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"api/rooms?userId={HttpEncode(userRemovedFromRoom.RemovedUserId.ToString())}&roomId={HttpEncode(userRemovedFromRoom.RoomId.ToString())}&senderId={HttpEncode(userRemovedFromRoom.SenderId.ToString())}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("api/users", cancellationToken);
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserInfo>>(cancellationToken: cancellationToken);
        return users!;
    }

    public async Task<UserInfo> LoginUserAsync(string userName, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"api/users?userName={HttpEncode(userName)}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserInfo>(cancellationToken: cancellationToken);
        return user!;
    }

    private string HttpEncode(string value)
    {
        return HttpUtility.UrlEncode(value);
    }
}
