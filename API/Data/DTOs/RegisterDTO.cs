using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public sealed class RegisterDTO
{
    [Required]
    public required string UserName { get; set; }
    [Required]
    public required string Password { get; set; }
}