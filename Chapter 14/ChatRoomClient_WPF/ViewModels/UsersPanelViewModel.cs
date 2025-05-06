using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using SharedContracts;
using SharedContracts.Notifications;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels;

public class UsersPanelViewModel : ViewModelBase
{
    private readonly IMessenger messenger;
    private readonly IChatRoomManagerModel chatRoomManagerModel;
    private readonly ICommunicationHelper communicationHelper;
    private bool isAddUsersPopupOpen;
    private RoomUserViewModel? selectedUser;

    public UsersPanelViewModel(IMessenger messenger,
        IChatRoomManagerModel chatRoomManagerModel,
        ICommunicationHelper communicationHelper)
    {
        this.messenger = messenger;
        this.chatRoomManagerModel = chatRoomManagerModel;
        this.communicationHelper = communicationHelper;

        messenger.Subscribe<RemoveRoomMessage>(OnRemoveRoom);
        messenger.Subscribe<RoomSelectedMessage>(OnRoomSelected);
        messenger.Subscribe<UserLeftChatNotification>(OnUserLeftChatNotification);
        messenger.Subscribe<UserJoinedChatNotification>(OnUserJoinedChatNotification);
        messenger.Subscribe<RoomUsersAddedMessage>(OnRoomUsersAdded);

        RemoveRoomUserCommand = new RelayCommand(RemoveRoomUser, CanRemoveRoomUser);
        AddRoomUserCommand = new RelayCommand(AddRoomUser, CanAddRoomUser);
    }

    public ObservableCollection<RoomUserViewModel> UsersList { get; } = [];

    #region Properties
    public bool IsAddUsersPopupOpen
    {
        get => isAddUsersPopupOpen;
        set
        {
            SetProperty(ref isAddUsersPopupOpen, value);
        }
    }

    public RoomUserViewModel? SelectedUser
    {
        get => selectedUser;
        set
        {
            SetProperty(ref selectedUser, value);
            ((RelayCommand)RemoveRoomUserCommand).RaiseCanExecuteChanged();
        }
    }
    #endregion

    #region Commands
    public ICommand AddRoomUserCommand { get; }

    private void AddRoomUser()
    {
        messenger.Publish(new AddingUsersMessage(chatRoomManagerModel.SelectedChatRoomId!.Value, chatRoomManagerModel.ChatUser!.UserId));
        IsAddUsersPopupOpen = true;
    }

    private bool CanAddRoomUser() => chatRoomManagerModel.SelectedChatRoomId != null;

    public ICommand RemoveRoomUserCommand { get; }

    private async void RemoveRoomUser()
    {
        if (SelectedUser != null)
        {
            await communicationHelper
                .ExecuteAsync(() => chatRoomManagerModel.RemoveUserAsync(chatRoomManagerModel.SelectedChatRoomId!.Value, SelectedUser.Id));
            UsersList.Remove(SelectedUser);
            SelectedUser = null;
            if (UsersList.Count == 0)
            {
                messenger.Publish(new RemoveRoomMessage());
            }
        }
    }

    private bool CanRemoveRoomUser() => SelectedUser != null;
    #endregion

    #region Message Handlers
    private void OnRoomSelected(RoomSelectedMessage message)
    {
        if (chatRoomManagerModel.SelectedChatRoomId != null)
        {
            RefreshRoomUsers();
        }
        else
        {
            UsersList.Clear();
        }
        ((RelayCommand)AddRoomUserCommand).RaiseCanExecuteChanged();
        ((RelayCommand)RemoveRoomUserCommand).RaiseCanExecuteChanged();
    }

    private async void OnRoomUsersAdded(RoomUsersAddedMessage message)
    {
        await communicationHelper
            .ExecuteAsync(() => chatRoomManagerModel.AddUsersAsync(message.RoomId, message.Users));

        foreach (var user in message.Users)
        {
            if (UsersList.Any(u => u.Id == user.UserId))
                continue;
            var userViewModel = new RoomUserViewModel(user);
            UsersList.Add(userViewModel);
        }
        IsAddUsersPopupOpen = false;
    }

    private void OnRemoveRoom(RemoveRoomMessage message)
    {
        if (chatRoomManagerModel.SelectedChatRoomId != null)
        {
            UsersList.Clear();
        }
    }

    private void OnUserJoinedChatNotification(UserJoinedChatNotification message)
    {
        var userInfo = new UserInfo(message.UserId, message.UserName);
        chatRoomManagerModel.AddUser(message.RoomId, userInfo);
        if (message.RoomId == chatRoomManagerModel.SelectedChatRoomId && !UsersList.Any(u => u.Id == message.UserId))
        {
            var userViewModel = new RoomUserViewModel(userInfo);
            UsersList.Add(userViewModel);
        }
    }

    private async void OnUserLeftChatNotification(UserLeftChatNotification message)
    {
        await chatRoomManagerModel.RemoveUserAsync(message.RoomId, message.UserId, false);
        if (message.RoomId == chatRoomManagerModel.SelectedChatRoomId)
        {
            var userViewModel = UsersList.FirstOrDefault(u => u.Id == message.UserId);
            if (userViewModel != null)
            {
                UsersList.Remove(userViewModel);
            }
        }
    }
    #endregion

    #region Private Methods
    private void RefreshRoomUsers()
    {
        if (chatRoomManagerModel.SelectedChatRoomId != null)
        {
            UsersList.Clear();
            foreach (var user in chatRoomManagerModel.GetUsers(chatRoomManagerModel.SelectedChatRoomId.Value))
            {
                var userViewModel = new RoomUserViewModel(user);
                UsersList.Add(userViewModel);
            }
        }
    }
    #endregion
}
