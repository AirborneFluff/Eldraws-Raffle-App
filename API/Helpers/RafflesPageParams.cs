namespace RaffleApi.Helpers;

public class RafflesPageParams : PaginationParams
{
    public DateTime? EndCloseDate { get; set; }
    public DateTime? StartCloseDate { get; set; }
}