using ChatRoomClient.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Windows;

namespace ChatRoomClient.ViewModels;

public class CommunicationHelper(ILogger<CommunicationHelper> logger) : ICommunicationHelper
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        await ExecuteInternalAsync(async () =>
        {
            await action();
            return true;
        });
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        return await ExecuteInternalAsync(action);
    }

    private async Task<T> ExecuteInternalAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning(ex, "Not Found: {Message}", ex.Message);
            ShowErrorMessage("The requested resource was not found. Please try again.");
            return default!;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
            ShowErrorMessage("An error occurred while communicating with the server. Please try again later.");
            return default!;
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Timeout error: {Message}", ex.Message);
            ShowErrorMessage("The request timed out. Please check your internet connection and try again.");
            return default!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            ShowErrorMessage("An unexpected error occurred. Please try again later.");
            return default!;
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
