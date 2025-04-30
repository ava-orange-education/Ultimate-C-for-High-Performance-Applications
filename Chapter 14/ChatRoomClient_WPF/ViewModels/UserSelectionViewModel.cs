using ChatRoomClient.ViewModels.Messages;
using SharedContracts;

namespace ChatRoomClient.ViewModels
{
    public class UserSelectionViewModel(UserInfo userInfo, IMessenger messenger) : ViewModelBase
    {
        private string? userName = userInfo.UserName;
        public string? UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                SetProperty(ref isSelected, value);
                messenger.Publish(new UserSelectionChangedMessage());
            }
        }

        public UserInfo UserInfo
        {
            get => userInfo;
        }
    }
}
