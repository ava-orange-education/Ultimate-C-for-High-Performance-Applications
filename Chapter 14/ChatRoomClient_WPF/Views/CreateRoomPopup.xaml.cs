using ChatRoomClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ChatRoomClient.Views;
/// <summary>
/// Interaction logic for CreateRoomPopup.xaml
/// </summary>
public partial class CreateRoomPopup : UserControl
{
    public CreateRoomPopup()
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
        var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<CreateRoomViewModel>();
        DataContext = viewModel;
    }
}
