using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PrazoCerto.ViewModels;

public abstract partial class NotificationPopupViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isNotificationOpen;
}