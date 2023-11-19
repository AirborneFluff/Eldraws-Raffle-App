using System.Text;
using Discord;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Helpers;

public static class RaffleEmbedBuilder
{
    public static EmbedBuilder GenerateEmbed(this Raffle raffle)
    {
        var embed = raffle.GenerateDescriptionEmbed();
        embed.AddPrizes(raffle);
        embed.AddWinners(raffle);
        embed.AddEntries(raffle);

        //embed.AddField("Trouble viewing this? Try the website...", $"({URL}/raffles/{raffle.Id}/preview)", false);

        var currentTime = DateTime.UtcNow.ToString("dd-MMM @ hh:mm tt");
        embed.Footer = new EmbedFooterBuilder()
            .WithText($"Last updated: {currentTime} UTC");

        return embed;
    }

    public static EmbedBuilder GenerateRollingEmbed(this Raffle raffle, int? rollValue = null, bool reRoll = false)
    {
        var embed = raffle.GenerateDescriptionEmbed();
        embed.AddPrizes(raffle);
        embed.AddRollValue(rollValue, !reRoll);
        embed.AddWinners(raffle, true);
        embed.AddEntries(raffle);

        var currentTime = DateTime.UtcNow.ToString("dd-MMM @ hh:mm tt");
        embed.Footer = new EmbedFooterBuilder()
            .WithText($"Last updated: {currentTime} UTC");

        return embed;
    }

    private static EmbedBuilder GenerateDescriptionEmbed(this Raffle raffle)
    {
        var descriptionSb = new StringBuilder();
        var openDate = raffle.OpenDate.ToString("dddd dd MMM");
        var closeDate = raffle.CloseDate.ToString("dddd dd MMM");
        var drawDate = raffle.DrawDate.ToString("dddd dd MMM");
        var drawTime = raffle.DrawDate.ToString("h tt");
        
        descriptionSb.AppendLine($"Open from: {openDate}");
        descriptionSb.AppendLine($"Closes at: {closeDate}");
        descriptionSb.AppendLine($"Drawing winners at: {drawTime} on {drawDate}");
        descriptionSb.AppendLine($"Tickets cost: {raffle.EntryCost} each");

        return new EmbedBuilder
        {
            Title = raffle.Title,
            Description = descriptionSb.ToString()
        };
    }

    private static EmbedBuilder AddPrizes(this EmbedBuilder embed, Raffle raffle)
    {
        var prizes = raffle.Prizes;
        if (!prizes.Any())
            return embed.AddField($"Prizes", "No listed prizes yet");
        
        var sb = new StringBuilder();
        foreach(var prize in prizes)
            sb.AppendLine(GetPrizeDescription(raffle, prize));
        
        embed.AddField($"Prizes", sb.ToString());
        
        return embed;
    }

    private static string GetPrizeDescription(Raffle raffle, RafflePrize prize)
    {
        var sb = new StringBuilder();
        
        var position = $"**{prize.Place.AddPositionalSynonym()}**";
        sb.Append(position.PadString(65, 75));

        var description = prize.Description;

        if (prize.DonationPercentage > 0)
        {
            var percentage = (int)(prize.DonationPercentage * 100);
            var value = (int)(raffle.GetTotalDonations() * prize.DonationPercentage);
            description = 
                $"{percentage}% of donations: " + value.AddDigitGroupSeperator();
        }

        sb.Append(description);
        return sb.ToString();
    }
    
    private static EmbedBuilder AddWinners(this EmbedBuilder embed, Raffle raffle, bool showPending = false)
    {
        var prizes = raffle.Prizes.OrderBy(p => p.Place)
            .ToArray();

        if (prizes.Length == 0) return embed;

        var sb = new StringBuilder();
    
        foreach (var prize in prizes)
        {
            var winner = raffle.Entries.FirstOrDefault(e => e.Tickets.Item1 <= prize.WinningTicketNumber && e.Tickets.Item2 >= prize.WinningTicketNumber);
            if (!showPending && winner == null) continue;

            var posStr = $"**{prize.Place.AddPositionalSynonym()}**";
            sb.Append(posStr.PadString(65, 75));
            
            if (winner != null)
            {
                sb.Append($"({prize.WinningTicketNumber}) ");
                sb.AppendLine(winner.Entrant?.Gamertag);
                continue;
            }
            
            sb.AppendLine("<a:threepointsanima:1005525060490117191>");
            
        }

        if (sb.Length == 0) return embed;
        embed.AddField($"Winners", sb.ToString());
    
        return embed;
    }
    
    private static EmbedBuilder AddRollValue(this EmbedBuilder embed, int? value, bool rollValid)
    {
        if (value == null) return embed;
        var sb = new StringBuilder();

        sb.Append(rollValid ? "Rolling..." : "Re-rolling...");
        sb.Append("<a:dices:1172979983321411676> ");
        sb.Append(value.ToString() ?? "...");
    
        embed.AddField("Roll",sb.ToString());
    
        return embed;
    }
    
    private static EmbedBuilder AddEntries(this EmbedBuilder embed, Raffle raffle)
    {
        var entries = raffle.Entries;
        if (!entries.Any()) return
            embed.AddField($"Entries", "Nobody has entered yet");
    
        var entryLines = new string[entries.Count];
        var entryPos = 0;
        foreach (var entry in entries)
        {
            entryLines[entryPos] = GetEntryDescription(entry);
            entryPos++;
        }
        
        var lineCount = entryLines.Count();
        var linePos = 0;
        var pagePos = 1;
    
        var fieldSb = new StringBuilder();
        while (linePos < lineCount)
        {
            fieldSb.Append(entryLines[linePos]);
            linePos++;

            if (linePos % 10 != 0) continue;
            
            embed.AddField($"Page: {pagePos}", fieldSb.ToString());
            fieldSb.Clear();
            pagePos++;
        }
        
        return embed.AddField($"Page: {pagePos}", fieldSb.ToString());
    }

    private static string GetEntryDescription(RaffleEntry entry)
    {
        if (entry.Tickets.Item1 == 0 || entry.Tickets.Item2 == 0) return String.Empty;
        
        var sb = new StringBuilder();
        var gamertag = entry.Entrant?.Gamertag ?? "Unknown";
    
        sb.Append(gamertag.PadString(250, 300));
        sb.Append(entry.Tickets.Item1);
        sb.Append(" - ");
        sb.Append(entry.Tickets.Item2);
        sb.AppendLine();
    
        return sb.ToString();
    }
}