using System.Windows.Threading;

namespace ChatRoomClient.ViewModels.Messages
{
    public class Messenger : IMessenger
    {
        //Capture the UI thread dispatcher
        //assuming the Messenger is created during App startup
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        private readonly Dictionary<Type, List<Delegate>> subscribers = [];

        public void Subscribe<T>(Action<T> action)
        {
            if (subscribers.TryGetValue(typeof(T), out var actions))
            {
                actions.Add(action);
            }
            else
            {
                subscribers[typeof(T)] = [action];
            }
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            if (subscribers.TryGetValue(typeof(T), out var actions))
            {
                actions.Remove(action);
                if (actions.Count == 0)
                {
                    subscribers.Remove(typeof(T));
                }
            }
        }

        public void Publish<T>(T message)
        {
            if (subscribers.TryGetValue(typeof(T), out var actions))
            {
                foreach (var action in actions)
                {
                    dispatcher.Invoke(() =>
                    {
                        ((Action<T>)action)(message);
                    });
                }
            }
        }
    }
}
