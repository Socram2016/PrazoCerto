using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PrazoCerto.ViewModels;

public abstract partial class NotificationPopupViewModel : ViewModelBase
{
    [RelayCommand]
    private void ClosePopup()
    {
        IsExpiredNotificationOpen = false;
    }
}