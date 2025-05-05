using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.Logging;
using SharedContracts.Events;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels;

public class MessagesPanelViewModel : ViewModelBase
{
    private readonly IMessenger messenger;
    private readonly IChatRoomManagerModel chatRoomManagerModel;
    private readonly ILogger<MessagesPanelViewModel> logger;
    private string? messageInput;
    private bool isMessageInputEnabled;

    public MessagesPanelViewModel(IMessenger messenger,
        IChatRoomManagerModel chatRoomManagerModel,
        ILogger<MessagesPanelViewModel> logger)
    {
        this.messenger = messenger;
        this.chatRoomManagerModel = chatRoomManagerModel;
        this.logger = logger;
        this.messenger.Subscribe<RoomSelectedMessage>(OnRoomSelected);
        this.messenger.Subscribe<ChatMessageReceivedEvent>(OnChatMessageRecieved);
        this.messenger.Subscribe<RemoveRoomMessage>(OnRemoveRoom);

        MessagesPanel.CollectionChanged += MessagesPanel_CollectionChanged;

        SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
    }

    public ObservableCollection<UserMessageViewModel> MessagesPanel { get; } = [];

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

    public bool IsMessageInputEnabled
    {
        get => isMessageInputEnabled;
        set
        {
            if (!value)
            {
                MessageInput = string.Empty;
            }
            SetProperty(ref isMessageInputEnabled, value);
        }
    }
    #endregion

    #region Message Handlers
    private void OnRemoveRoom(RemoveRoomMessage message)
    {
        MessagesPanel.Clear();
    }

    private void OnChatMessageRecieved(ChatMessageReceivedEvent message)
    {
        if (message.RoomId == chatRoomManagerModel.SelectedChatRoomId)
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
        chatRoomManagerModel.ReceiveMessage(message);
    }

    public void OnRoomSelected(RoomSelectedMessage message)
    {
        IsMessageInputEnabled = chatRoomManagerModel.SelectedChatRoomId != null;

        if (chatRoomManagerModel.SelectedChatRoomId != null)
        {
            RefreshRoomMessages();
        }
        else
        {
            MessagesPanel.Clear();
            MessageInput = string.Empty;
        }
    }
    #endregion

    #region Event Handlers
    private void MessagesPanel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        messenger.Publish(new ScrollMessagesToBottomMessage());
    }
    #endregion

    #region Commands
    public ICommand SendMessageCommand { get; }

    private async void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageInput))
        {
            var message = await chatRoomManagerModel.AddMessageAsync(
                chatRoomManagerModel.SelectedChatRoomId!.Value,
                MessageInput);
            MessagesPanel.Add(new UserMessageViewModel(true, chatRoomManagerModel.ChatUser!.UserName, message!));
            MessageInput = string.Empty;
        }
    }

    private bool CanSendMessage() => !string.IsNullOrWhiteSpace(MessageInput);
    #endregion

    #region Private Methods
    private void RefreshRoomMessages()
    {
        if (chatRoomManagerModel.SelectedChatRoomId != null)
        {
            MessagesPanel.Clear();
            foreach (var message in chatRoomManagerModel.GetMessages(chatRoomManagerModel.SelectedChatRoomId.Value))
            {
                var isOwnedByUser = message.User.UserId == chatRoomManagerModel.ChatUser!.UserId;
                var messageViewModel = new UserMessageViewModel(isOwnedByUser,
                    message.User.UserName,
                    message);
                MessagesPanel.Add(messageViewModel);
            }
        }
    }
    #endregion
}
