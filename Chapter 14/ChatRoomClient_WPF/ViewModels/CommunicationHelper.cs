using ChatRoomClient.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Windows;

namespace ChatRoomClient.ViewModels;

public class CommunicationHelper(ILogger<CommunicationHelper> logger) : ICommunicationHelper
{
    public async Task ExecuteRequestAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning(ex, "Not Found: {Message}", ex.Message);
            ShowErrorMessage("The requested resource was not found. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            ShowErrorMessage("An error occurred while communicating with the server. Please try again later.");
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Timeout error: {Message}", ex.Message);
            ShowErrorMessage("The request timed out. Please check your internet connection and try again.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            ShowErrorMessage("An unexpected error occurred. Please try again later.");
        }
    }

    private static void ShowErrorMessage(string message)
    {
        MessageBox.Show(message,
            "Communication Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
