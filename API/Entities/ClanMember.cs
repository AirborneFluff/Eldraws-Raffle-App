using System.ComponentModel.DataAnnotations.Schema;

namespace RaffleApi.Entities;

public class ClanMember
{
    public int Id { get; set; }
    public AppUser? Member { get; set; }
    public string MemberId { get; set; }
    public Clan? Clan { get; set; }
    public int ClanId { get; set; }
}