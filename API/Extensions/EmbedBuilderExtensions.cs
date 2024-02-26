using System.Text;
using Discord;
using RaffleApi.Helpers;

namespace RaffleApi.Extensions;

public static class EmbedBuilderExtensions
{
    public static readonly int MaxFieldLength = 1024;
    public static readonly int MaxEmbedLength = 5900;
    
    public static void AddLinedField(this EmbedBuilder embed, string title, LookAheadEnumerator<string> lines, int characterLimit = Int32.MaxValue)
    {
        characterLimit = characterLimit > MaxFieldLength ? MaxFieldLength : characterLimit;
        var sb = new StringBuilder();

        while (sb.Length < characterLimit)
        {
            if (!lines.HasNext) break;
            
            var item = lines.Next;
            var itemLength = item.Length + 2;

            if (sb.Length + itemLength > characterLimit) break;
            if (embed.Length + itemLength > MaxEmbedLength) break;
            sb.AppendLine(item);
            lines.MoveNext();
        }

        var fieldContent = sb.ToString();
        if (fieldContent.Length != 0) embed.AddField(title, fieldContent);
    }
}