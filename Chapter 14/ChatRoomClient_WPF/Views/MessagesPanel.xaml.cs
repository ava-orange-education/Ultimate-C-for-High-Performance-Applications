using ChatRoomClient.ViewModels;
using ChatRoomClient.ViewModels.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for MessagesPanel.xaml
/// </summary>
public partial class MessagesPanel : UserControl
{
    private readonly IMessenger messenger;

    public MessagesPanel()
    {
        InitializeComponent();

        messenger = ((App)Application.Current).ServiceProvider.GetRequiredService<IMessenger>();
        messenger.Subscribe<ScrollMessagesToBottomMessage>(OnScrollToBottom);

        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            Loaded += OnLoaded;
        }
    }

    private void OnScrollToBottom(ScrollMessagesToBottomMessage message)
    {
        MessagesScrollViewer.ScrollToBottom();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<MessagesPanelViewModel>();
        DataContext = viewModel;
    }
}
