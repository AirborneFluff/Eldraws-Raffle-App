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
        CreateMap<UpdateClanDTO, Clan>();
        CreateMap<Clan, ClanDTO>();
        CreateMap<Clan, ClanInfoDTO>();
        
        CreateMap<NewRaffleDTO, Raffle>();
        CreateMap<Raffle, RaffleDTO>();

        CreateMap<NewEntrantDTO, Entrant>();
        CreateMap<Entrant, EntrantDTO>();
        CreateMap<Entrant, EntrantInfoDTO>();
        
        CreateMap<NewRaffleEntryDTO, RaffleEntry>();
        CreateMap<RaffleEntry, RaffleEntryDTO>();
        CreateMap<RaffleEntry, RaffleEntryInfoDTO>();

        CreateMap<NewRafflePrizeDTO, RafflePrize>();
        CreateMap<UpdateRafflePrizeDTO, RafflePrize>();
        CreateMap<RafflePrize, RafflePrizeInfoDTO>()
            .ForMember(dest => dest.DonationPercentage, opt =>
                opt.MapFrom(src => src.DonationPercentage));
    }
}