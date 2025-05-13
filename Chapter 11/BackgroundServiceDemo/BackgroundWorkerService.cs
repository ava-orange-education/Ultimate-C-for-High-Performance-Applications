using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServiceDemo;
internal class BackgroundWorkerService : BackgroundService
{
    private readonly ILogger logger;

    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Background service starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(5000, stoppingToken);
                this.logger.LogInformation($"Background service processing at {DateTime.Now}.");
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                this.logger.LogError("Background service error: " + ex.ToString());
            }
        }

        this.logger.LogInformation("Background service stopping.");
    }
}
