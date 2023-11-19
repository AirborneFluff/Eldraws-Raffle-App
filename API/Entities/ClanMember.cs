namespace RaffleApi.Entities;

public class ClanMember
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public required string MemberId { get; set; }
    
    public Clan? Clan { get; set; }
    public AppUser? Member { get; set; }
}