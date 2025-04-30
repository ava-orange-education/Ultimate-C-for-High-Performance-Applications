
namespace ChatRoomClient.ViewModels.Messages;

public interface IMessenger
{
    void Publish<T>(T message);
    void Subscribe<T>(Action<T> action);
    void Unsubscribe<T>(Action<T> action);
}