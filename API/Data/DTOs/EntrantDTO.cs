﻿namespace RaffleApi.Data.DTOs;

public class EntrantDTO
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public required string Gamertag { get; set; }
}