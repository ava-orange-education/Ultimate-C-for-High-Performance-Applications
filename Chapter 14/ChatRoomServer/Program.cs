using ChatRoomServer.Interfaces;
using ChatRoomServer.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblyContaining<Program>());

builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddSingleton<IUserSocketService, UserSocketService>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<IChatRoomStore, ChatRoomStore>();
var app = builder.Build();

app.UseWebSockets();
app.UseRouting();
app.MapControllers();

await app.RunAsync();
