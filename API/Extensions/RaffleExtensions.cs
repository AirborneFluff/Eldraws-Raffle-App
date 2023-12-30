using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class RaffleExtensions
{
    public static Tuple<int, int> GetTickets(this Raffle raffle, int donation)
    {
        var requiredTickets = (int) Math.Floor((decimal) donation / raffle.EntryCost);
        if (requiredTickets == 0) return new Tuple<int, int>(0, 0);
        if (raffle.Entries.Count == 0) return new Tuple<int, int>(1, requiredTickets);

        var lastTicket = raffle.GetLastTicket();
        return new Tuple<int, int>(lastTicket + 1, lastTicket + requiredTickets);
    }

    public static void RedistributeTickets(this Raffle raffle)
    {
        var count = 1;
        foreach (var entry in raffle.Entries)
        {
            var ticketCount = entry.Donation / raffle.EntryCost;
            if (ticketCount == 0) continue;
            
            entry.LowTicket = count;
            entry.HighTicket = count + ticketCount - 1;
            count += ticketCount;
        }
    }

    public static int GetTotalDonations(this Raffle raffle)
    {
        return raffle.Entries.Sum(entry => entry.Donation);
    }

    public static int GetLastTicket(this Raffle raffle)
    {
        return raffle.Entries.Max(e => e.Tickets.Item2);
    }
    
    public static Entrant? GetEntrantFromTicket(this Raffle raffle, int ticketNumber)
    {
        if (!raffle.Entries.Any()) throw new Exception("Raffle entries not included");
        
        var winner = raffle.Entries.FirstOrDefault(e => e.Tickets.Item1 <= ticketNumber && e.Tickets.Item2 >= ticketNumber);
        return winner?.Entrant;
    }

    public static bool HasEntrantAlreadyWon(this Raffle raffle, Entrant entrant)
    {
        foreach (var prize in raffle.Prizes)
        {
            if (prize.WinningTicketNumber == null) continue;
            
            var winner = GetEntrantFromTicket(raffle, (int) prize.WinningTicketNumber);
            if (winner == null) continue;
            
            if (winner.Id == entrant.Id) return true;
        }

        return false;
    }
}