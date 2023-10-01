using System.ComponentModel.DataAnnotations;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class ClanDTO
{
    public int Id { get; set; }
    public string Name { get; set; }

    public MemberDTO Owner { get; set; }
    public IEnumerable<MemberDTO> Members { get; set; }
    public IEnumerable<EntrantInfoDTO> Entrants { get; set; }
    public IEnumerable<RaffleDTO> Raffles { get; set; }
}