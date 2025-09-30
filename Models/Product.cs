using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PrazoCerto.Models;

public class Product : ObservableObject
{
    private TimeSpan Tempo => ExpirationDate - DateTime.Now;
    private string _timeRemaining;

    public string TimeRemaining
    {
        get => _timeRemaining;
        set => SetProperty(ref _timeRemaining, value);
    }

    private string _name;

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
                OnPropertyChanged(nameof(Tempo));
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
                   int amount)
    {
        _name = name;
        _codeBar = codeBar;
        _expirationDate = expirationDate;

        var daysToExpiration = (ExpirationDate - DateTime.Now).Days;

        if (daysToExpiration <= 0) _timeRemaining = "Vencido";
        else _timeRemaining = $"{(ExpirationDate - DateTime.Now).Days} dias";
        
        _amount = amount;
    }

}