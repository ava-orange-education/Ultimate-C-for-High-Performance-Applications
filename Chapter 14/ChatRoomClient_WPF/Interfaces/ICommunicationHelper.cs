namespace ChatRoomClient.Interfaces;

public interface ICommunicationHelper
{
    Task ExecuteAsync(Func<Task> action);
    Task<T> ExecuteAsync<T>(Func<Task<T>> action);
}