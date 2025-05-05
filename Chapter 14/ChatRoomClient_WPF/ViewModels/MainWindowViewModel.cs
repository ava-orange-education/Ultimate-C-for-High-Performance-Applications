using ChatRoomClient.Interfaces;
using ChatRoomClient.ViewModels.Messages;
using System.Windows.Input;

namespace ChatRoomClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string? chatUserName;
    private bool isLoggedIn;

    private readonly IMessenger messenger;
    private readonly IChatRoomManagerModel chatRoomManagerModel;
    private readonly ICommunicationHelper communicationHelper;

    #region Constructor
    public MainWindowViewModel(IMessenger messenger,
        IChatRoomManagerModel chatRoomManagerModel,
        ICommunicationHelper communicationHelper)
    {
        this.messenger = messenger;
        this.chatRoomManagerModel = chatRoomManagerModel;
        this.communicationHelper = communicationHelper;

        // Command initialization
        ConfirmChatUserNameCommand = new RelayCommand(ConfirmChatUserName, CanConfirmChatUserName);
        LogoutCommand = new RelayCommand(Logout);
    }
    #endregion

    #region Properties

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
    #endregion

    #region Commands
    public ICommand ConfirmChatUserNameCommand { get; }

    private async void ConfirmChatUserName()
    {
        await communicationHelper
            .ExecuteRequestAsync(() => chatRoomManagerModel.LoginAsync(ChatUserName!));
        IsLoggedIn = true;
        ChatUserName = null;

        messenger.Publish(new LoadChatRoomsMessage());
    }

    private bool CanConfirmChatUserName() => !string.IsNullOrWhiteSpace(ChatUserName);

    public ICommand LogoutCommand { get; }

    private async void Logout()
    {
        await communicationHelper
            .ExecuteRequestAsync(chatRoomManagerModel.LogoutAsync);
    }
    #endregion
}
