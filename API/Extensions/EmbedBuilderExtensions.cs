﻿using System.Text;
using Discord;
using RaffleApi.Helpers;

namespace RaffleApi.Extensions;

public static class EmbedBuilderExtensions
{
    public static readonly int MaxFieldLength = 1024;
    
    public static void AddLinedField(this EmbedBuilder embed, string title, LookAheadEnumerator<string> lines, int characterLimit = Int32.MaxValue)
    {
        characterLimit = characterLimit > MaxFieldLength ? MaxFieldLength : characterLimit;
        var sb = new StringBuilder();

        while (sb.Length < characterLimit)
        {
            if (!lines.MoveNext()) break;
            
            var item = lines.Current;
            var itemLength = item.Length + 2;
            
            if (sb.Length + itemLength > characterLimit) break;
            sb.AppendLine(item);
        }

        var fieldContent = sb.ToString();
        if (fieldContent.Length != 0) embed.AddField(title, fieldContent);
    }
}