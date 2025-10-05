using System;
using System.Linq;

namespace PrazoCerto.Models;

public abstract class ToCheck
{
    public static bool IsInt(string? number)
    {
        return int.TryParse(number, out _);
    }

    public static bool IsAllFilled(params string[]? args)
    {
        return args != null && args.All(arg => !string.IsNullOrEmpty(arg));
    }

    public static bool IsValidCodeBar(string? codeBar)
    {
        return long.TryParse(codeBar, out _);
    }

    public static bool IsValidDate(string? day, string? month, string? year)
    {
        if (string.IsNullOrEmpty(day) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(year)) return false;
        
        
        var dateString = $"{year}-{month}-{day}";
        return DateTime.TryParse(dateString, out _);
    }
    
}