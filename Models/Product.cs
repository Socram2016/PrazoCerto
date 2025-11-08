using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PrazoCerto.Models;

public class Product : ObservableObject
{
    private TimeSpan Time => ExpirationDate - DateTime.Now;
    private string _timeRemaining = string.Empty;

    public string TimeRemaining
    {
        get => _timeRemaining;
        set => SetProperty(ref _timeRemaining, value);
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private long _codeBar;
    public long CodeBar
    {
        get => _codeBar;
        set => SetProperty(ref _codeBar, value);
    }

    private DateTime _expirationDate;
    public DateTime ExpirationDate
    {
        get => _expirationDate;
        set
        {
            if (_expirationDate != value)
            {
                SetProperty(ref _expirationDate, value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Time));
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    public Product(string name,
                   long codeBar,
                   DateTime expirationDate,
                   int amount = 0)
    {
        Name = name;
        CodeBar = codeBar;
        ExpirationDate = expirationDate;

        var daysToExpiration = (ExpirationDate - DateTime.Now).Days;

        TimeRemaining = daysToExpiration <= 0 ?
            "Vencido" :
            $"{(ExpirationDate-DateTime.Now).Days:0000} dias";
        
        Amount = amount;
}

}