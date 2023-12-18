namespace RaffleApi.Helpers;

public class EntrantParams : PaginationParams
{
    public string? Gamertag { get; set; }
    public string? OrderBy { get; set; }
}