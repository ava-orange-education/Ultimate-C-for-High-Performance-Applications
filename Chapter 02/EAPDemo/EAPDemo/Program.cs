namespace EAPDemo
{

    using System;
    using System.Threading;

    internal class CompletedEventArgs : EventArgs
    {
        public bool IsSuccessful { get; }
        public string Message { get; }

        public CompletedEventArgs(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

    internal class MessageTimer
    {
        private TimeSpan _delay;
        private string _message;
        private Timer _timer;

        public event EventHandler<CompletedEventArgs> Completed;

        public MessageTimer(TimeSpan delay, string message)
        {
            _delay = delay;
            _message = message;
        }

        public void Start()
        {
            _timer = new Timer(OnTimer, null, _delay, TimeSpan.Zero);
        }

        private void OnTimer(object state)
        {
            Completed?.Invoke(state, new CompletedEventArgs(true, _message));
        }
    }

    internal class Program
    {
        static void Main()
        {
            var messageTimer = new MessageTimer(TimeSpan.FromSeconds(2), "Timer completed successfully.");
            messageTimer.Completed += MessageTimer_Completed;
            messageTimer.Start();
            Console.WriteLine("Timer started.");
            Console.ReadLine();
        }

        private static void MessageTimer_Completed(object? sender, CompletedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }

}
