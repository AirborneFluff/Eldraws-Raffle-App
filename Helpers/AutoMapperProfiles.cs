using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using AutoMapper;

namespace RaffleApi.Helpers;

public sealed class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, AppUserDTO>();
        CreateMap<ClanDTO, Clan>();
        CreateMap<Clan, ClanDTO>();
    }
}