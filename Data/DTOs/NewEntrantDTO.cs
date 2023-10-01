using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewEntrantDTO
{
    [Required]
    public string Gamertag { get; set; }
}