using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.Logging;
using SharedContracts;
using SharedContracts.Events;
using SharedContracts.Notifications;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string? messageInput;
    private string? chatUserName;
    private bool isLoggedIn;
    private bool isCreateRoomPopupOpen;
    private bool isAddUsersPopupOpen;
    private ChatRoomViewModel? selectedChatRoom;
    private RoomUserViewModel? selectedUser;

    private readonly IMessenger messenger;
    private readonly IChatRoomManagerModel chatRoomManagerModel;
    private readonly ICommunicationHelper communicationHelper;
    private readonly ILogger<MainWindowViewModel> logger;

    #region Collections
    public ObservableCollection<ChatRoomViewModel> ChatRoomsList { get; } = [];
    public ObservableCollection<UserMessageViewModel> MessagesPanel { get; } = [];
    public ObservableCollection<RoomUserViewModel> UsersList { get; } = [];
    #endregion

    #region Constructor
    public MainWindowViewModel(IMessenger messenger,
        IChatRoomManagerModel chatRoomManagerModel,
        ICommunicationHelper communicationHelper,
        ILogger<MainWindowViewModel> logger)
    {
        this.messenger = messenger;
        this.chatRoomManagerModel = chatRoomManagerModel;
        this.communicationHelper = communicationHelper;
        this.logger = logger;
        messenger.Subscribe<ChatRoomCreatedMessage>(OnChatRoomCreated);
        messenger.Subscribe<RoomUsersAddedMessage>(OnRoomUsersAdded);
        messenger.Subscribe<ChatMessageReceived>(OnChatMessageRecieved);
        messenger.Subscribe<UserJoinedChatNotification>(OnUserJoinedChatNotification);
        messenger.Subscribe<UserLeftChatNotification>(OnUserLeftChatNotification);

        MessagesPanel.CollectionChanged += MessagesPanel_CollectionChanged;

        // Command initialization
        SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
        CreateChatRoomCommand = new RelayCommand(CreateChatRoom);
        AddRoomUserCommand = new RelayCommand(AddRoomUser, CanAddRoomUser);
        RemoveRoomUserCommand = new RelayCommand(RemoveRoomUser, CanRemoveRoomUser);
        ConfirmChatUserNameCommand = new RelayCommand(ConfirmChatUserName, CanConfirmChatUserName);
        LogoutCommand = new RelayCommand(Logout);
    }
    #endregion

    #region Properties
    public string? MessageInput
    {
        get => messageInput;
        set
        {
            SetProperty(ref messageInput, value);
            ((RelayCommand)SendMessageCommand).RaiseCanExecuteChanged();
        }
    }

    public ChatRoomViewModel? SelectedChatRoom
    {
        get => selectedChatRoom;
        set
        {
            SetProperty(ref selectedChatRoom, value);
            ((RelayCommand)AddRoomUserCommand).RaiseCanExecuteChanged();

            RefreshRoomMessages();
            RefreshRoomUsers();
            if (selectedChatRoom != null)
                selectedChatRoom.FontWeight = FontWeights.Normal;
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

    public string? ChatUserName
    {
        get => chatUserName;
        set
        {
            SetProperty(ref chatUserName, value);
            ((RelayCommand)ConfirmChatUserNameCommand).RaiseCanExecuteChanged();
        }
    }

    public bool IsLoggedIn
    {
        get => isLoggedIn;
        set
        {
            SetProperty(ref isLoggedIn, value);
        }
    }

    public bool IsCreateRoomPopupOpen
    {
        get => isCreateRoomPopupOpen;
        set
        {
            SetProperty(ref isCreateRoomPopupOpen, value);
            if (!value)
            {
                messenger.Publish(new ResetCreateRoomMessage());
            }
        }
    }

    public bool IsAddUsersPopupOpen
    {
        get => isAddUsersPopupOpen;
        set
        {
            SetProperty(ref isAddUsersPopupOpen, value);
        }
    }
    #endregion

    #region Commands
    public ICommand SendMessageCommand { get; }

    private async void SendMessage()
    {
        //Todo: Implement message sending logic
        if (!string.IsNullOrWhiteSpace(MessageInput))
        {
            var message = await chatRoomManagerModel.AddMessageAsync(
                SelectedChatRoom!.Id,
                MessageInput);
            MessagesPanel.Add(new UserMessageViewModel(true, null, message!));
            MessageInput = string.Empty;
        }
    }

    private bool CanSendMessage() => !string.IsNullOrWhiteSpace(MessageInput);

    public ICommand CreateChatRoomCommand { get; }

    private void CreateChatRoom()
    {
        IsCreateRoomPopupOpen = true;
    }

    public ICommand AddRoomUserCommand { get; }

    private void AddRoomUser()
    {
        messenger.Publish(new AddingUsersMessage(SelectedChatRoom!.Id, chatRoomManagerModel.ChatUser!.UserId));
        IsAddUsersPopupOpen = true;
    }

    private bool CanAddRoomUser() => SelectedChatRoom != null;

    public ICommand RemoveRoomUserCommand { get; }

    private async void RemoveRoomUser()
    {
        if (SelectedUser != null)
        {
            await communicationHelper
                .ExecuteRequestAsync(() => chatRoomManagerModel.RemoveUserAsync(SelectedChatRoom!.Id, SelectedUser.Id));
            UsersList.Remove(SelectedUser);
            SelectedUser = null;
            if (UsersList.Count == 0)
            {
                RemoveRoom();
            }
        }
    }

    private bool CanRemoveRoomUser() => SelectedUser != null;

    public ICommand ConfirmChatUserNameCommand { get; }

    private async void ConfirmChatUserName()
    {
        await communicationHelper
            .ExecuteRequestAsync(() => chatRoomManagerModel.LoginAsync(ChatUserName!));
        IsLoggedIn = true;
        ChatUserName = null;

        await LoadChatRooms();
    }

    private bool CanConfirmChatUserName() => !string.IsNullOrWhiteSpace(ChatUserName);

    public ICommand LogoutCommand { get; }

    private async void Logout()
    {
        await communicationHelper
            .ExecuteRequestAsync(() => chatRoomManagerModel.LogoutAsync());
    }
    #endregion

    #region Message Handlers
    private async void OnChatRoomCreated(ChatRoomCreatedMessage message)
    {
        var newRoom = new ChatRoomViewModel
        {
            Name = message.RoomName!,
            Id = message.RoomId,
            FontWeight = FontWeights.Normal
        };
        ChatRoomsList.Add(newRoom);
        IsCreateRoomPopupOpen = false;

        await communicationHelper
            .ExecuteRequestAsync(() => chatRoomManagerModel.AddChatRoomAsync(message.RoomName!, message.RoomId));
        SelectedChatRoom = newRoom;
    }

    private async void OnRoomUsersAdded(RoomUsersAddedMessage message)
    {
        await communicationHelper
            .ExecuteRequestAsync(() => chatRoomManagerModel.AddUsersAsync(message.RoomId, message.Users));

        foreach (var user in message.Users)
        {
            if (UsersList.Any(u => u.Id == user.UserId))
                continue;
            var userViewModel = new RoomUserViewModel(user);
            UsersList.Add(userViewModel);
        }
        IsAddUsersPopupOpen = false;
    }

    private void OnChatMessageRecieved(ChatMessageReceived message)
    {
        if (message != null)
        {
            var room = ChatRoomsList.FirstOrDefault(r => r.Id == message.RoomId);
            if (room != null)
            {
                if (room == SelectedChatRoom)
                {
                    var user = chatRoomManagerModel.GetUsers(message.RoomId).FirstOrDefault(u => u.UserId == message.User.UserId);
                    if (user != null)
                    {
                        var isOwnedByUser = user.UserId == chatRoomManagerModel.ChatUser!.UserId;
                        var messageViewModel = new UserMessageViewModel(isOwnedByUser, user.UserName, message);
                        MessagesPanel.Add(messageViewModel);
                    }
                    else
                    {
                        logger.LogDebug("User {userId} not found in room {roomId}", message.User.UserId, message.RoomId);
                    }
                }
                else
                {
                    room.FontWeight = FontWeights.Bold;
                }
            }
            else
            {
                logger.LogDebug("Room {roomId} not found", message.RoomId);
            }
        }
    }

    private async void OnUserJoinedChatNotification(UserJoinedChatNotification message)
    {
        var room = ChatRoomsList.FirstOrDefault(r => r.Id == message.RoomId);
        if (room != null)
        {
            var userInfo = new UserInfo(message.UserId, message.UserName);
            chatRoomManagerModel.AddUser(message.RoomId, userInfo);
            if (room == SelectedChatRoom && !UsersList.Any(u => u.Id == message.UserId))
            {
                var userViewModel = new RoomUserViewModel(userInfo);
                UsersList.Add(userViewModel);
            }
        }
        else
        {
            await LoadChatRooms();
        }
    }

    private async void OnUserLeftChatNotification(UserLeftChatNotification message)
    {
        var room = ChatRoomsList.FirstOrDefault(r => r.Id == message.RoomId);
        if (room != null)
        {
            await chatRoomManagerModel.RemoveUserAsync(message.RoomId, message.UserId, false);
            if (room == SelectedChatRoom)
            {
                var userViewModel = UsersList.FirstOrDefault(u => u.Id == message.UserId);
                if (userViewModel != null)
                {
                    UsersList.Remove(userViewModel);
                }
            }
            if (message.UserId == chatRoomManagerModel.ChatUser!.UserId)
            {
                if (room == SelectedChatRoom)
                {
                    RemoveRoom();
                    SelectedChatRoom = ChatRoomsList.FirstOrDefault();
                }
                ChatRoomsList.Remove(room);
            }
        }
        else
        {
            logger.LogDebug("Room {roomId} not found", message.RoomId);
        }
    }
    #endregion

    #region Event Handlers
    private void MessagesPanel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        messenger.Publish(new ScrollMessagesToBottomMessage());
    }
    #endregion

    #region Private Methods
    private void RefreshRoomUsers()
    {
        if (selectedChatRoom != null)
        {
            UsersList.Clear();
            foreach (var user in chatRoomManagerModel.GetUsers(selectedChatRoom.Id))
            {
                var userViewModel = new RoomUserViewModel(user);
                UsersList.Add(userViewModel);
            }
        }
    }

    private void RefreshRoomMessages()
    {
        if (selectedChatRoom != null)
        {
            MessagesPanel.Clear();
            foreach (var message in chatRoomManagerModel.GetMessages(selectedChatRoom.Id))
            {
                var isOwnedByUser = message.User.UserId == chatRoomManagerModel.ChatUser!.UserId;
                var messageViewModel = new UserMessageViewModel(isOwnedByUser,
                    message.User.UserName,
                    message);
                MessagesPanel.Add(messageViewModel);
            }
        }
    }

    private void RemoveRoom()
    {
        if (SelectedChatRoom != null)
        {
            chatRoomManagerModel.RemoveChatRoom(SelectedChatRoom.Id);
            ChatRoomsList.Remove(SelectedChatRoom);
            SelectedChatRoom = null;
            MessagesPanel.Clear();
            UsersList.Clear();
        }
    }

    private async Task LoadChatRooms()
    {
        var chatRooms = await chatRoomManagerModel.GetRoomsForUserAsync();
        foreach (var room in chatRooms)
        {
            if (ChatRoomsList.Any(r => r.Id == room.RoomId))
                continue;

            var chatRoom = new ChatRoomViewModel
            {
                Name = room.RoomName,
                Id = room.RoomId,
                FontWeight = FontWeights.Normal
            };
            ChatRoomsList.Add(chatRoom);
        }
        SelectedChatRoom ??= ChatRoomsList.FirstOrDefault();
    }
    #endregion
}
