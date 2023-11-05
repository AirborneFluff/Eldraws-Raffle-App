namespace RaffleApi.Extensions;

public static class IntExtensions
{
    public static string AddPositionalSynonym(this int val)
    {
        return val switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => $"{val}th"
        };
    }

    public static string AddNumberMagnitude(this int val)
    {
        var abs = Math.Abs(val);
        var rounder = Math.Pow(10, 1);
        var sign = val < 0 ? "-" : ""; // will also work for Negetive numbers

        var powers = new Dictionary<char, double>
        {
            //{'Q',Math.Pow(10, 15)},
            //{'T',Math.Pow(10, 12)},
            //{'B',Math.Pow(10, 9)},
            {'M',Math.Pow(10, 6)},
            {'K', 1000},
        };

        foreach(var mag in powers)
            if (abs >= mag.Value) return $"{sign}{abs / mag.Value}{mag.Key}";
        
        return val.ToString();
    }

    public static string AddDigitGroupSeperator(this int val)
    {
        return String.Format("{0:n0}", val);
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T value)
    {
        var index = 0;
        var comparer = EqualityComparer<T>.Default; // or pass in as a parameter
        foreach (T item in source)
        {
            if (comparer.Equals(item, value)) return index;
            index++;
        }
        return -1;
    }
}