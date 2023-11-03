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
        // embed.AddWinners(raffle);
        // embed.AddEntries(raffle);

        //embed.AddField("Trouble viewing this? Try the website...", $"[FluffStuff](https://www.fluffstuff.uk/raffles/{raffle.Id}/preview)", false);

        var currentTime = DateTime.Now.ToString("dd-MMM @ hh:mm tt");
        embed.Footer = new EmbedFooterBuilder()
            .WithText($"Last updated: {currentTime} GMT");

        return embed;
    }

    private static EmbedBuilder GenerateDescriptionEmbed(this Raffle raffle)
    {
        var descriptionSb = new StringBuilder();
        var openDate = raffle.OpenDate.ToString("d MMM");
        var closeDate = raffle.CloseDate.ToString("d MMM");
        var drawDate = raffle.DrawDate.ToString("d MMM");
        var drawTime = raffle.DrawDate.ToString("h tt");
        
        descriptionSb.AppendLine($"From: {openDate} to {closeDate}");
        descriptionSb.AppendLine($"We will draw winners at around {drawTime} on {drawDate}");
        descriptionSb.AppendLine($"Tickets cost {raffle.EntryCost} each");

        return new EmbedBuilder
        {
            Title = raffle.Title,
            Description = descriptionSb.ToString()
        };
    }

    private static EmbedBuilder AddPrizes(this EmbedBuilder embed, Raffle raffle)
    {
        var prizes = raffle.Prizes;
        if (!prizes.Any()) return embed;
        
        var prizesSb = new StringBuilder();
        foreach(var prize in prizes)
        {
            var posStr = $"**{prize.Place.AddPositionalSynonym()}**";
            prizesSb.Append(posStr.PadString(65, 75));
            var descLines = prize.Description?.Split('\n');
            if (descLines == null) continue;
            
            foreach (var line in descLines)
            {
                if (descLines.First() == line)
                {
                    prizesSb.AppendLine(line);
                    continue;
                }
                prizesSb.AppendLine(new String("").PadString(65, 75) + line);
            }
        }
        if (prizes.Any()) embed.AddField($"Prizes", prizesSb.ToString(), false);
        
        else embed.AddField($"Prizes", "No listed prizes yet", false);
        
        return embed;
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
    // public static EmbedBuilder AddEntries(this EmbedBuilder embed, Raffle raffle)
    // {
    //     var winningTickets = raffle.Prizes.Select(p => p.WinningTicketNumber).ToList();
    //     var entries = raffle.Entries;
    //
    //     var entryLines = new string[entries.Count];
    //     var entryPos = 0;
    //     foreach (var entry in entries)
    //     {
    //         var entrySb = new StringBuilder();
    //         var gamertag = entry.Entrant?.Gamertag;
    //         if (gamertag == null) continue;
    //
    //         entrySb.Append(gamertag.PadString(250, 300));
    //         entrySb.Append(entry.Tickets.Item1);
    //         entrySb.Append(" - ");
    //         entrySb.Append(entry.Tickets.Item2.ToString());
    //
    //         entryLines[entryPos] = entrySb.ToString();
    //         entryPos++;
    //     }
    //     var lineCount = entryLines.Count();
    //     var linePos = 0;
    //     var pagePos = 1;
    //
    //     var fieldSb = new StringBuilder();
    //     while (linePos < lineCount)
    //     {
    //         fieldSb.AppendLine(entryLines[linePos]);
    //         linePos++;
    //
    //         if (linePos % 10 == 0)
    //         {
    //             embed.AddField($"Page: {pagePos}", fieldSb.ToString(), false);
    //             fieldSb.Clear();
    //             pagePos++;
    //         }
    //     }
    //     
    //     embed.AddField($"Page: {pagePos}", fieldSb.ToString(), false); // Final page
    //
    //     return embed;
    // }
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