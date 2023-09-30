using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using AutoMapper;

namespace RaffleApi.Helpers;

public sealed class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, AppUserDTO>();
        CreateMap<AppUser, MemberDTO>();

        CreateMap<ClanMember, ClanDTO>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.ClanId))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Clan!.Name))
            .ForMember(dest => dest.Members, opt =>
                opt.MapFrom(src => src.Clan!.Members));
        
        CreateMap<ClanMember, MemberDTO>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.Member!.UserName));
            
        CreateMap<NewClanDTO, Clan>();
        CreateMap<Clan, ClanDTO>();
            // .ForMember(dest => dest.Owner, opt =>
            //     opt.MapFrom(src => src.Owner));
        
        CreateMap<NewRaffleDTO, Raffle>();
        CreateMap<Raffle, RaffleDTO>();

        CreateMap<NewEntrantDTO, Entrant>();
        CreateMap<Entrant, EntrantDTO>();
        
        CreateMap<NewRaffleEntryDTO, RaffleEntry>();
        CreateMap<RaffleEntry, RaffleEntryDTO>();
    }
}