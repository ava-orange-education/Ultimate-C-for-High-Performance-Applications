using ChatRoomClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for ChatRoomsPanel.xaml
/// </summary>
public partial class ChatRoomsPanel : UserControl
{
    public ChatRoomsPanel()
    {
        InitializeComponent();

        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            Loaded += OnLoaded;
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<ChatRoomsPanelViewModel>();
        DataContext = viewModel;
    }
}
