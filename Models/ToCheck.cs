using System;

public class ToCheck
{
    public static bool IsInt(string number)
    {
        

        if (Int32.TryParse(number, out _)) return true;

        return false;
    }

    public static bool IsFilled(params string[] args)
    {
        // Return false if is not filled 

        bool isAllFilled = true;

        foreach (var arg in args)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                isAllFilled = false;
                break;
            }
        }

        return isAllFilled;
    }

    public static bool IsValidCodeBar(string codeBar)
    {
        return Int64.TryParse(codeBar, out _);
    }

    public static bool IsValidDate(string day, string month, string year)
    {
        string dateString = $"{year}-{month}-{day}";
        return DateTime.TryParse(dateString, out _);
    }
}