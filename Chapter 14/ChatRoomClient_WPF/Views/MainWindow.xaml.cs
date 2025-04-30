using ChatRoomClient.ViewModels;
using ChatRoomClient.ViewModels.Messages;
using System.Windows;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IMessenger messenger;

    public MainWindow(MainWindowViewModel mainWindowViewModel, IMessenger messenger)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
        this.messenger = messenger;
        messenger.Subscribe<ScrollMessagesToBottomMessage>(OnScrollMessagesToBottom);
    }

    private void OnScrollMessagesToBottom(ScrollMessagesToBottomMessage message)
    {
        MessagesScrollViewer.ScrollToEnd();
    }
}