using System.Text;
using Discord;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Helpers;

public static class RaffleEmbedBuilder
{
    private static readonly string URL = "https://eldraws.co.uk/";
    
    public static EmbedBuilder GenerateEmbed(this Raffle raffle)
    {
        var embed = raffle.GenerateDescriptionEmbed();
        embed.AddPrizes(raffle);
        // embed.AddWinners(raffle);
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
                $"{percentage}% of donations: " + value;
        }

        sb.Append(description);
        return sb.ToString();
    }
    
    //
    // public static EmbedBuilder AddWinners(this EmbedBuilder embed, Raffle raffle)
    // {
    //     var wonPrizes = raffle.Prizes
    //         .OrderBy(p => p.Place)
    //         .Where(p => p.WinningTicketNumber != null && p.WinningTicketNumber != 0)
    //         .ToList();
    //
    //     if (wonPrizes == null) return embed;
    //
    //     var winnersSb = new StringBuilder();
    //
    //     foreach (var prize in wonPrizes)
    //     {
    //         var winner = raffle.Entries.FirstOrDefault(e => e.Tickets.Item1 <= prize.WinningTicketNumber && e.Tickets.Item2 >= prize.WinningTicketNumber);
    //         if (winner == null) continue;
    //
    //         var posStr = $"**{prize.Place.AddPositionalSynonym()}**";
    //         winnersSb.Append(posStr.PadString(65, 75));
    //         winnersSb.AppendLine(winner.Entrant?.Gamertag);
    //     }
    //
    //     embed.AddField($"Winners", winnersSb.ToString(), false);
    //
    //     return embed;
    // }
    //
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