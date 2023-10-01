using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public sealed class RegisterDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}