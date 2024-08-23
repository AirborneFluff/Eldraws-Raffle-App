using System.ComponentModel.DataAnnotations.Schema;

namespace RaffleApi.Data.DTOs;

public class RaffleEntryInfoDTO
{
    public int Id { get; set; }
        
    public EntrantInfoDTO? Entrant { get; set; }

    public int Donation { get; set; }
    public DateTime InputDate { get; set; } = DateTime.Now;
    public bool Complimentary { get; set; }

    [NotMapped]
    public Tuple<int, int>? Tickets { get; set; }
}