using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class RaffleExtensions
{
    public static Tuple<int, int> GetTickets(this Raffle raffle, int donation)
    {
        var requiredTickets = (int) Math.Floor((decimal) donation / raffle.EntryCost);
        if (requiredTickets == 0) return new Tuple<int, int>(0, 0);
        if (raffle.Entries.Count == 0) return new Tuple<int, int>(1, requiredTickets);

        var lastTicket = raffle.Entries.Max(e => e.Tickets.Item2);
        return new Tuple<int, int>(lastTicket + 1, lastTicket + requiredTickets);
    }

    public static void RedistributeTickets(this Raffle raffle)
    {
        var count = 1;
        foreach (var entry in raffle.Entries)
        {
            var ticketCount = entry.Donation / raffle.EntryCost;
            entry.Tickets = new Tuple<int, int>(count, count + ticketCount - 1);
            count += ticketCount;
        }
    }

    public static int GetTotalDonations(this Raffle raffle)
    {
        return raffle.Entries.Sum(entry => entry.Donation);
    }
}