using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using AutoMapper;

namespace RaffleApi.Helpers;

public sealed class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, AppUserDTO>();
        
        CreateMap<NewClanDTO, Clan>();
        CreateMap<Clan, ClanDTO>();
        
        CreateMap<NewRaffleDTO, Raffle>();
        CreateMap<Raffle, RaffleDTO>();

        CreateMap<NewEntrantDTO, Entrant>();
        CreateMap<Entrant, EntrantDTO>();
        
        CreateMap<NewRaffleEntryDTO, RaffleEntry>();
        CreateMap<RaffleEntry, RaffleEntryDTO>();
    }
}