using ChatRoomClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for UsersPanel.xaml
/// </summary>
public partial class UsersPanel : UserControl
{
    public UsersPanel()
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
        var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<UsersPanelViewModel>();
        DataContext = viewModel;
    }
}
