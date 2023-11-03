using System.Text;

namespace RaffleApi.Helpers;

public static class PixelCounter
{
    public static string PadString(this string s, int maxLength, int paddingPosition)
    {
        var totalStringLength = s.StringPixelWidth();
        
        if (totalStringLength > maxLength)
        {
            var newLength = totalStringLength;
            var dotAppend = "...";
            var dotAppendLength = dotAppend.StringPixelWidth();
            
            for (var i = s.Length - 1; newLength + dotAppendLength > maxLength; i--)
            {
                newLength -= s[i].CharPixelWidth();
                s = s.Remove(i);
            }

            s = s + dotAppend;
            totalStringLength = newLength + dotAppend.StringPixelWidth();
        }

        var paddingNeeded = paddingPosition - totalStringLength;
        var emCount = (int) (paddingNeeded / PixelWidths['\u2003']);
        var hairLineCount = (int) ((paddingNeeded % PixelWidths['\u2003']) / PixelWidths['\u200a']);
        var entrySb = new StringBuilder();
        entrySb.Append(s);

        for (var i = 0; i < emCount; i++) entrySb.Append('\u2003');
        for (var i = 0; i < hairLineCount; i++) entrySb.Append('\u200a');

        return entrySb.ToString();
    }

    private static float StringPixelWidth(this string s)
    {
        float pixelCount = 0;
        foreach (var c in s)
            pixelCount += c.CharPixelWidth();
        
        return pixelCount;
    }

    private static float CharPixelWidth(this char c)
    {
        if (c == '*') return 0;
        if (c == '_') return 0;
        try
        {
            return PixelWidths[c];
        }
        catch
        {
            return 15.7f; // Average
        }
    }

    private static readonly Dictionary<char, float> PixelWidths = new Dictionary<char, float>
    {
        {'a', 14.7f},
        {'b', 15.8f},
        {'c', 13.7f},
        {'d', 15.9f},
        {'e', 14.7f},
        {'f', 9.0f},
        {'g', 15.1f},
        {'h', 15.7f},
        {'i', 7.0f},
        {'j', 7.0f},
        {'k', 14.0f},
        {'l', 7.0f},
        {'m', 24.6f},
        {'n', 15.7f},
        {'o', 15.7f},
        {'p', 15.9f},
        {'q', 15.8f},
        {'r', 10.3f},
        {'s', 13.0f},
        {'t', 9.9f},
        {'u', 15.7f},
        {'v', 14.1f},
        {'w', 21.8f},
        {'x', 13.9f},
        {'y', 14.4f},
        {'z', 13.2f},
        {'A', 20.7f},
        {'B', 16.4f},
        {'C', 18.5f},
        {'D', 21.0f},
        {'E', 15.2f},
        {'F', 14.3f},
        {'G', 20.8f},
        {'H', 21.2f},
        {'I', 8.0f},
        {'J', 10.4f},
        {'K', 18.2f},
        {'L', 14.0f},
        {'M', 27.2f},
        {'N', 21.2f},
        {'O', 22.4f},
        {'P', 15.9f},
        {'Q', 22.4f},
        {'R', 17.0f},
        {'S', 15.1f},
        {'T', 17.4f},
        {'U', 20.4f},
        {'V', 20.4f},
        {'W', 30.8f},
        {'X', 18.8f},
        {'Y', 18.8f},
        {'Z', 17.9f},
        {'1', 10.7f},
        {'2', 15.8f},
        {'3', 15.7f},
        {'4', 17.8f},
        {'5', 16.0f},
        {'6', 17.0f},
        {'7', 16.0f},
        {'8', 17.1f},
        {'9', 17.0f},
        {'0', 18.5f},
        {' ', 6.8f}, // Standard Space
        {'.', 6.6f}, // Dot
        {'\u2003', 30.0f}, // Em Whitespace
        {'\u200a', 2.3f}, // Hairspace
    };
}