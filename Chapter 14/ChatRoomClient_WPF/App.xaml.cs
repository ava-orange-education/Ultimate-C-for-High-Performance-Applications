using ChatRoomClient.Clients;
using ChatRoomClient.Configuration;
using ChatRoomClient.Interfaces;
using ChatRoomClient.Models;
using ChatRoomClient.ViewModels;
using ChatRoomClient.ViewModels.Messages;
using ChatRoomClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ChatRoomClient;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    public IConfiguration Configuration { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceCollection services = new ServiceCollection();
        Configuration = BuildConfiguration();
        ConfigureServices(services, Configuration);
        ServiceProvider = services.BuildServiceProvider();
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow.Show();
    }

    private static IConfiguration BuildConfiguration()
    {
        var result = new ConfigurationBuilder();
        result.Sources.Add(new JsonConfigurationSource
        {
            Path = "ClientConfig.json",
            Optional = true,
            ReloadOnChange = true
        });
        return result.Build();
    }

    private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddDebug();
        });

        services.Configure<ServerConfig>(configuration.GetSection("ServiceConfig"));

        services.AddHttpClient<IChatRoomApiClient, ChatRoomApiClient>(client =>
        {
            var serverConfig = configuration.GetSection("ServiceConfig").Get<ServerConfig>();
            client.BaseAddress = serverConfig!.ToUri();
        });

        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<CreateRoomPopup>();
        services.AddSingleton<CreateRoomViewModel>();
        services.AddSingleton<AddUsersPopup>();
        services.AddSingleton<AddUsersViewModel>();
        services.AddSingleton<ChatRoomsPanel>();
        services.AddSingleton<ChatRoomsPanelViewModel>();
        services.AddSingleton<MessagesPanel>();
        services.AddSingleton<MessagesPanelViewModel>();
        services.AddSingleton<UsersPanel>();
        services.AddSingleton<UsersPanelViewModel>();
        services.AddSingleton<IMessenger, Messenger>();
        services.AddSingleton<IChatRoomManagerModel, ChatRoomManagerModel>();
        services.AddSingleton<IWebSocketsClient, WebSocketsClient>();
        services.AddTransient<ICommunicationHelper, CommunicationHelper>();
    }
}

