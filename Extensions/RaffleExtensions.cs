﻿using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class RaffleExtensions
{
    public static Tuple<int, int> GetTickets(this Raffle raffle, int donation)
    {
        var requiredTickets = (int) Math.Floor((decimal) donation / raffle.EntryCost);
        if (raffle.Entries.Count == 0) return new Tuple<int, int>(1, requiredTickets + 1);

        var lastTicket = raffle.Entries.Max(e => e.Tickets.Item2);
        return new Tuple<int, int>(lastTicket + 1, lastTicket + requiredTickets);
    }
}