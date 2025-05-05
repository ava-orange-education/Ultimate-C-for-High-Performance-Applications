using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.Logging;
using SharedContracts.Events;
using SharedContracts.Notifications;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels;

public class ChatRoomsPanelViewModel : ViewModelBase
{
    private readonly IMessenger messenger;
    private readonly IChatRoomManagerModel chatRoomManagerModel;
    private readonly ICommunicationHelper communicationHelper;
    private readonly ILogger<ChatRoomsPanelViewModel> logger;
    private ChatRoomViewModel? selectedChatRoom;
    private bool isCreateRoomPopupOpen;

    public ChatRoomsPanelViewModel(IMessenger messenger,
        IChatRoomManagerModel chatRoomManagerModel,
        ICommunicationHelper communicationHelper,
        ILogger<ChatRoomsPanelViewModel> logger)
    {
        this.messenger = messenger;
        this.messenger.Subscribe<LoadChatRoomsMessage>(OnLoadChatRooms);
        this.messenger.Subscribe<RemoveRoomMessage>(OnRemoveRoom);
        this.messenger.Subscribe<UserLeftChatNotification>(OnUserLeftChatNotification);
        this.messenger.Subscribe<UserJoinedChatNotification>(OnUserJoinedChatNotification);
        this.messenger.Subscribe<ChatMessageReceivedEvent>(OnChatMessageReceived);
        this.messenger.Subscribe<ChatRoomCreatedMessage>(OnChatRoomCreated);

        this.chatRoomManagerModel = chatRoomManagerModel;
        this.communicationHelper = communicationHelper;
        this.logger = logger;
        CreateChatRoomCommand = new RelayCommand(CreateChatRoom);
    }

    public ObservableCollection<ChatRoomViewModel> ChatRoomsList { get; } = [];

    #region Properties
    public ChatRoomViewModel? SelectedChatRoom
    {
        get => selectedChatRoom;
        set
        {
            SetProperty(ref selectedChatRoom, value);
            chatRoomManagerModel.SelectedChatRoomId = value?.Id;

            messenger.Publish(new RoomSelectedMessage());

            if (selectedChatRoom != null)
                selectedChatRoom.FontWeight = FontWeights.Normal;
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
    #endregion

    #region Commands
    public ICommand CreateChatRoomCommand { get; }

    private void CreateChatRoom()
    {
        IsCreateRoomPopupOpen = true;
    }
    #endregion

    #region Message Handlers
    private async void OnLoadChatRooms(LoadChatRoomsMessage message)
    {
        await LoadChatRoomsAsync();
    }

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

    private void OnChatMessageReceived(ChatMessageReceivedEvent message)
    {
        if (message.RoomId != chatRoomManagerModel.SelectedChatRoomId)
        {
            var room = ChatRoomsList.FirstOrDefault(r => r.Id == message.RoomId);
            if (room != null)
            {
                room.FontWeight = FontWeights.Bold;
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
        if (room == null)
        {
            await LoadChatRoomsAsync();
        }
    }

    private void OnUserLeftChatNotification(UserLeftChatNotification message)
    {
        var room = ChatRoomsList.FirstOrDefault(r => r.Id == message.RoomId);
        if (room != null)
        {
            if (message.UserId == chatRoomManagerModel.ChatUser!.UserId)
            {
                if (room == SelectedChatRoom)
                {
                    OnRemoveRoom(new RemoveRoomMessage());
                }
                ChatRoomsList.Remove(room);
            }
        }
        else
        {
            logger.LogDebug("Room {roomId} not found", message.RoomId);
        }
    }

    private void OnRemoveRoom(RemoveRoomMessage message)
    {
        if (SelectedChatRoom != null)
        {
            chatRoomManagerModel.RemoveChatRoom(SelectedChatRoom.Id);
            ChatRoomsList.Remove(SelectedChatRoom);
            SelectedChatRoom = ChatRoomsList.FirstOrDefault();
        }
    }
    #endregion

    #region Private Methods
    private async Task LoadChatRoomsAsync()
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