namespace RaffleApi.Data.DTOs;

public class RollWinnerDTO
{
    public EntrantInfoDTO? Winner { get; set; }
    public bool Reroll { get; set; }
    public int TicketNumber { get; set; }
}