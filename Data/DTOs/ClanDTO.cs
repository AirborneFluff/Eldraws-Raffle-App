using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public sealed class ClanDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}