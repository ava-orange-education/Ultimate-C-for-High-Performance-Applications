using ChatRoomClient.ViewModels.Messages;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels
{
    public class CreateRoomViewModel : ViewModelBase
    {
        private string? roomName;

        public string? RoomName
        {
            get => roomName;
            set
            {
                SetProperty(ref roomName, value);
                ((RelayCommand)CreateRoomCommand).RaiseCanExecuteChanged();
            }
        }

        private readonly IMessenger messenger;

        public ICommand CreateRoomCommand { get; }

        public CreateRoomViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            messenger.Subscribe<ResetCreateRoomMessage>(OnReset);

            // Command initialization
            CreateRoomCommand = new RelayCommand(ExecuteCreateRoom, CanExecuteCreateRoom);
        }

        private void ExecuteCreateRoom()
        {
            messenger.Publish(new ChatRoomCreatedMessage(RoomName));
        }

        private bool CanExecuteCreateRoom()
        {
            return !string.IsNullOrWhiteSpace(RoomName);
        }

        public void OnReset(ResetCreateRoomMessage message)
        {
            RoomName = string.Empty;
        }
    }
}
