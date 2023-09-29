﻿using System.ComponentModel.DataAnnotations.Schema;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public class RaffleEntryDTO
{
    public int Id { get; set; }
    public int RaffleId { get; set; }
        
    public int EntrantId { get; set; }
    public EntrantDTO? Entrant { get; set; }

    public int Donation { get; set; }
    public DateTime InputDate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public Tuple<int, int>? Tickets { get; set; }
}