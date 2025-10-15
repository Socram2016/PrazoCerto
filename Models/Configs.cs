using CommunityToolkit.Mvvm.ComponentModel;

namespace PrazoCerto.Models;

public class Configs : ObservableObject
{
    private string _timeToExpirationNotif;
    
    public Configs(string timeToExpirationNotif)
    {
        _timeToExpirationNotif = timeToExpirationNotif;
    }
}