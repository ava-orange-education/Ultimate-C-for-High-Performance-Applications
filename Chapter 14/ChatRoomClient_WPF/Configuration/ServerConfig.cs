namespace ChatRoomClient.Configuration;

public class ServerConfig
{
    public string? Host { get; set; }

    public int Port { get; set; }

    public Uri ToUri()
    {
        if (string.IsNullOrEmpty(Host))
        {
            throw new InvalidOperationException("Host is not set.");
        }
        if (Port == 0)
        {
            throw new InvalidOperationException("Port is not set.");
        }
        return new Uri($"http://{Host}:{Port}");
    }
}
