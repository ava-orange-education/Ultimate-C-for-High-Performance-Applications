using System.Windows;

namespace ChatRoomClient.ViewModels;

public class ChatRoomViewModel : ViewModelBase
{
    private string name = string.Empty;
    private Guid id;
    private FontWeight fontWeight = FontWeights.Normal;

    public string Name
    {
        get => name;
        set
        {
            SetProperty(ref name, value);
        }
    }

    public Guid Id { get => id; set => id = value; }

    public FontWeight FontWeight
    {
        get => fontWeight;
        set
        {
            SetProperty(ref fontWeight, value);
        }
    }
}
