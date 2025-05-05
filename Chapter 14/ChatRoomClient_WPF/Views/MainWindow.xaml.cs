using ChatRoomClient.ViewModels;
using ChatRoomClient.ViewModels.Messages;
using System.Windows;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel, IMessenger messenger)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
    }
}