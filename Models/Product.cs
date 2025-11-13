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
                   DateTime expirationDate,
                   int amount = 0,
                   long codeBar = 000000)
    {
        Name = name;
        CodeBar = codeBar;
        ExpirationDate = expirationDate;
        
        TimeRemaining = UpdateTimeRemaining(expirationDate);
        
        Amount = amount;
        
    }

    public string UpdateTimeRemaining(DateTime expirationDate)
    {
        var daysToExpiration = (ExpirationDate - DateTime.Now).Days;
        
        if (daysToExpiration <= 0)
        {
            return "Vencido";
        }
        
        int yearsToExpiration = (int)Math.Floor(daysToExpiration/365.0);
        daysToExpiration -= yearsToExpiration * 365;

        int monthsToExpiration = (int)Math.Floor(daysToExpiration/30.44);
        daysToExpiration -= monthsToExpiration * 30;

        return  $"{yearsToExpiration} anos, {monthsToExpiration} meses, {daysToExpiration} dias";

        
        
    }
    
}