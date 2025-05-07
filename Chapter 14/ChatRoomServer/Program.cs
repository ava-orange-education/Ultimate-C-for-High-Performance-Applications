using ChatRoomServer.ErrorHandlers;
using ChatRoomServer.Interfaces;
using ChatRoomServer.Services;
using SharedContracts.Commands;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly,
    typeof(SendChatMessageCommand).Assembly));

builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

//Services that store state are registered as singletons
builder.Services.AddSingleton<IUserSocketService, UserSocketService>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<IChatRoomStore, ChatRoomStore>();

//Services that do not store state are registered as transient
builder.Services.AddTransient<IBroadcastService, BroadcastService>();

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

//Shut down all connections when the service is stopped
app.Lifetime.ApplicationStopping.Register(async () =>
{
    var userSocketService = app.Services.GetRequiredService<IUserSocketService>();
    await userSocketService.CloseAllSocketsAsync();
});

app.UseWebSockets();
app.UseRouting();
app.MapControllers();
app.UseExceptionHandler();

await app.RunAsync();
