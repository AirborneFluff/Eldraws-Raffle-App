namespace RaffleApi.Data.DTOs;

public sealed class AppUserDTO
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Token { get; set; }
}