using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels
{
    public class AddUsersViewModel : ViewModelBase
    {
        private readonly IMessenger messenger;
        private readonly IChatRoomApiClient chatRoomApiClient;
        private Guid roomId;

        public ObservableCollection<UserSelectionViewModel> AvailableUsers { get; } = [];

        public ICommand AddSelectedUsersCommand { get; }

        public AddUsersViewModel(IMessenger messenger,
            IChatRoomApiClient chatRoomApiClient)
        {
            messenger.Subscribe<AddingUsersMessage>(OnAddingUsers);
            messenger.Subscribe<UserSelectionChangedMessage>(OnUserSelectionChanged);

            AddSelectedUsersCommand = new RelayCommand(AddSelectedUsers, CanAddSelectedUsers);
            this.messenger = messenger;
            this.chatRoomApiClient = chatRoomApiClient;
        }

        private async void OnAddingUsers(AddingUsersMessage message)
        {
            roomId = message.RoomId;
            AvailableUsers.Clear();
            var allUsers = await chatRoomApiClient.GetAllUsersAsync(CancellationToken.None);
            foreach (var user in allUsers)
            {
                if (user.UserId != message.CurrentUserId)
                {
                    AvailableUsers.Add(new UserSelectionViewModel(user, messenger));
                }
            }
        }

        private void OnUserSelectionChanged(UserSelectionChangedMessage message)
        {
            ((RelayCommand)AddSelectedUsersCommand).RaiseCanExecuteChanged();
        }

        private void AddSelectedUsers()
        {
            var selectedUsers = AvailableUsers.Where(user => user.IsSelected).Select(user => user.UserInfo).ToList();
            if (selectedUsers.Count > 0)
            {
                messenger.Publish(new RoomUsersAddedMessage(roomId, selectedUsers));
            }
        }

        public bool CanAddSelectedUsers()
        {
            return AvailableUsers.Any(user => user.IsSelected);
        }
    }
}
