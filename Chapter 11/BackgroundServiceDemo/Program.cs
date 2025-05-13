using BackgroundServiceDemo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();

        //Register hosted service
        services.AddHostedService<BackgroundWorkerService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    });

IHost host = builder.Build();
await host.RunAsync();