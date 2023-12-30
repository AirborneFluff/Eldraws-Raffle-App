namespace RaffleApi.Helpers;

public sealed class RaffleEntryParams : PaginationParams
{
    public string OrderBy { get; set; } = "descending";
}