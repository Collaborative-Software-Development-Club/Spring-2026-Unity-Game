using System;

public static class StringUtils
{
    private static readonly string[] suffixes = { "", "K", "M", "B", "T" };

    public static string AbbreviateNumber(int number)
    {
        return AbbreviateNumber((double)number);
    }

    public static string AbbreviateNumber(double number)
    {
        double value = number;
        int index = 0;

        while (value >= 1000 && index < suffixes.Length - 1)
        {
            value /= 1000d;
            index++;
        }

        string formatted = (value % 1 == 0)
            ? ((long)value).ToString() 
            : value.ToString("0.##");        

        return formatted + suffixes[index];
    }

    public static string PlaceSeparators(string stringToSeparate, string separator = " ")
    {
        var temp = "";
    
        for (var i = 0; i < stringToSeparate.Length; i++)
        {
            temp += stringToSeparate[i];

            if (i + 1 < stringToSeparate.Length && 
                char.IsLower(stringToSeparate[i]) && 
                char.IsUpper(stringToSeparate[i + 1]))
            {
                temp += separator;
            }
        }

        return temp;
    }
}