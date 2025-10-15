using System;
using System.IO;
using CommunityToolkit.Mvvm.Input;

namespace PrazoCerto.ViewModels;

public partial class ConfigPageViewModel : ViewModelBase
{
    [RelayCommand]
    public void SaveConfig()
    {
        File.WriteAllText(ProgramConfigPath, Convert.ToString(ConfigToExpirationNotif));
    }
}