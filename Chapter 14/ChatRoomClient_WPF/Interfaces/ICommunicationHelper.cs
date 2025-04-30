namespace ChatRoomClient.Interfaces;

public interface ICommunicationHelper
{
    Task ExecuteRequestAsync(Func<Task> action);
}