using System.Text;
using Discord;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Helpers;

public static class RaffleEmbedBuilder
{
    private static readonly string URL = "https://eldraws.co.uk/";
    
    public static EmbedBuilder GenerateEmbed(this Raffle raffle, bool showWinners = false, bool showRoll = false, int? rollValue = null)
    {
        var embed = raffle.GenerateDescriptionEmbed();
        embed.AddPrizes(raffle);
        if (showRoll) embed.AddRollValue(rollValue);
        if (showWinners) embed.AddWinners(raffle);
        embed.AddEntries(raffle);

        //embed.AddField("Trouble viewing this? Try the website...", $"({URL}/raffles/{raffle.Id}/preview)", false);

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
        if (!prizes.Any()) return
            embed.AddField($"Prizes", "No listed prizes yet", false);
        
        var sb = new StringBuilder();
        foreach(var prize in prizes)
            sb.AppendLine(GetPrizeDescription(raffle, prize));
        
        embed.AddField($"Prizes", sb.ToString(), false);
        
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
    
    private static EmbedBuilder AddWinners(this EmbedBuilder embed, Raffle raffle)
    {
        var prizes = raffle.Prizes.OrderBy(p => p.Place)
            .ToArray();

        if (prizes.Length == 0) return embed;

        var sb = new StringBuilder();
    
        foreach (var prize in prizes)
        {
            var winner = raffle.Entries.FirstOrDefault(e => e.Tickets.Item1 <= prize.WinningTicketNumber && e.Tickets.Item2 >= prize.WinningTicketNumber);

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
    
        embed.AddField($"Winners", sb.ToString(), false);
    
        return embed;
    }
    
    private static EmbedBuilder AddRollValue(this EmbedBuilder embed, int? value)
    {
        var sb = new StringBuilder();

        sb.Append("Rolling... ");
        sb.Append("<a:dices:1172979983321411676> ");
        sb.Append(value.ToString() ?? "...");
    
        embed.AddField("Roll",sb.ToString(), false);
    
        return embed;
    }
    
    private static EmbedBuilder AddEntries(this EmbedBuilder embed, Raffle raffle)
    {
        var entries = raffle.Entries;
        if (!entries.Any()) return
            embed.AddField($"Entries", "Nobody has entered yet", false);
    
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
            
            embed.AddField($"Page: {pagePos}", fieldSb.ToString(), false);
            fieldSb.Clear();
            pagePos++;
        }
        
        embed.AddField($"Page: {pagePos}", fieldSb.ToString(), false); // Final page
    
        return embed;
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
    //
    // public static EmbedBuilder RollEmoji(this EmbedBuilder embed, int rollPlace, int winningTicket, bool showWinner)
    // {
    //     var field = embed.Fields.FirstOrDefault(f => f.Name == "Prizes");
    //     if (field == null) return embed;
    //
    //     var lines = field.Value.ToString()?.Split('\n');
    //     if (lines == null) return embed;
    //
    //     var rollLine = lines.FirstOrDefault(l => l.Contains($"**{rollPlace.AddPositionalSynonym()}**"));
    //     var rollLinePos = lines.IndexOf(rollLine);
    //     
    //     if (rollLine == null) return embed;
    //     if (!showWinner)
    //         lines[rollLinePos] = rollLine.PadString(320, 350) + "<a:threepointsanima:1005525060490117191>\n";
    //     if (showWinner)
    //         lines[rollLinePos] = rollLine.PadString(320, 350) + winningTicket + '\n';
    //
    //     var lineSb = new StringBuilder();
    //     foreach(var line in lines) lineSb.Append(line);
    //
    //     field.Value = lineSb.ToString();
    //     return embed;
    // }
}