using System.Text;
using Discord;

namespace RaffleApi.Utilities;

public static class EmbedUtilities
{
    public static List<EmbedFieldBuilder> CreateLinedFields(string fieldName, IEnumerable<string> lines, bool isInline = false)
    {
        var sb = new StringBuilder();
        using var enumerator = lines.GetEnumerator();
        var fieldBuilders = new List<EmbedFieldBuilder>();

        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            var itemLength = item.Length + 2;

            if (sb.Length + itemLength > EmbedFieldBuilder.MaxFieldValueLength)
            {
                var fieldContent = sb.ToString();
                if (fieldContent.Length != 0)
                    fieldBuilders.Add(new EmbedFieldBuilder()
                    {
                        Name = fieldName,
                        Value = fieldContent,
                        IsInline = isInline
                    });
                sb.Clear();
            }

            sb.AppendLine(item);
        }
        
        var content = sb.ToString();
        if (content.Length != 0)
            fieldBuilders.Add(new EmbedFieldBuilder()
            {
                Name = fieldName,
                Value = content,
                IsInline = isInline
            });

        return fieldBuilders;
    }
    
}