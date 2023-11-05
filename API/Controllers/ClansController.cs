using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ValidateUser))]
[Authorize]
public sealed class ClansController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public ClansController(IMapper mapper, UnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    
    [HttpPost]
    public async Task<ActionResult<ClanDTO>> CreateNewClan(NewClanDTO clan)
    {
        var clanCheck = await _unitOfWork.ClanRepository.GetByName(clan.Name);
        if (clanCheck != null) return Conflict(clan.Name);
        
        var user = HttpContext.GetUser();
        var newClan = _mapper.Map<Clan>(clan);
        newClan.OwnerId = user.Id;
        
        _unitOfWork.ClanRepository.Add(newClan);
        if (!await _unitOfWork.Complete()) return BadRequest("Issue creating new clan");

        var clanMember = new ClanMember
        {
            MemberId = user.Id,
            ClanId = newClan.Id
        };
        
        newClan.Members.Add(clanMember);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<ClanInfoDTO>(newClan));
        
        return BadRequest("Issue adding clan");
    }
    
    
    [HttpPut("{clanId:int}")]
    [ServiceFilter(typeof(ValidateClanOwner))]
    public async Task<ActionResult<ClanDTO>> UpdateClan(UpdateClanDTO clanDto, int clanId)
    {
        var clan = HttpContext.GetClan();

        _mapper.Map(clanDto, clan);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<ClanInfoDTO>(clan));
        
        return BadRequest("Issue updating clan");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClanInfoDTO>>> GetUserClans()
    {
        var user = HttpContext.GetUser();
        var clans = await _unitOfWork.ClanRepository.GetAllForUser(user.Id);
        if (clans.Count == 0) return Ok(Enumerable.Empty<ClanInfoDTO>());

        var clansResult = clans.Select(clan => _mapper.Map<ClanInfoDTO>(clan));
        
        return Ok(clansResult);
    }
    
    [HttpGet("{clanId:int}")]
    [ServiceFilter(typeof(ValidateClanMember))]
    public async Task<ActionResult<IEnumerable<ClanInfoDTO>>> GetClan(int clanId)
    {
        return Ok(_mapper.Map<ClanDTO>(HttpContext.GetClan()));
    }
    
    [HttpDelete("{clanId:int}")]
    [ServiceFilter(typeof(ValidateClanOwner))]
    public async Task<ActionResult<ClanDTO>> DeleteClan(int clanId)
    {
        var clan = HttpContext.GetClan();
        
        _unitOfWork.ClanRepository.Remove(clan);
        if (await _unitOfWork.Complete()) return Ok();
        
        return BadRequest("Issue deleting clan");
    }
    
    [HttpPost("{clanId:int}/members/{memberId}")]
    [ServiceFilter(typeof(ValidateClanOwner))]
    public async Task<ActionResult<ClanDTO>> AddMember(string memberId, int clanId)
    {
        var clan = HttpContext.GetClan();
        if (clan.Members.FirstOrDefault(m => m.MemberId == memberId) != null)
            return Conflict("This user is already a member of this clan");

        var member = await _userManager.FindByIdAsync(memberId);
        if (member == null) return NotFound("No user found by that Id");

        var clanMember = new ClanMember
        {
            ClanId = clanId,
            MemberId = member.Id
        };
        
        clan.Members.Add(clanMember);
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<ClanDTO>(clan));
        
        return BadRequest();
    }
    
    [HttpDelete("{clanId:int}/members/{memberId}")]
    [ServiceFilter(typeof(ValidateClanOwner))]
    public async Task<ActionResult<ClanDTO>> RemoveMember(string memberId, int clanId)
    {
        var clan = HttpContext.GetClan();
        var user = HttpContext.GetUser();
        
        if (memberId == user.Id) return BadRequest("You cannot remove yourself from the clan");

        var clanMember = clan.Members.FirstOrDefault(m => m.MemberId == memberId);
        if (clanMember == null) return NotFound("That user is not a member of this clan");
        
        clan.Members.Remove(clanMember);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<ClanDTO>(clan));
        
        return BadRequest();
    }
    
    [HttpPost("{clanId:int}/entrants")]
    [ServiceFilter(typeof(ValidateClanMember))]
    public async Task<ActionResult<ClanDTO>> AddEntrant([FromBody] NewEntrantDTO entrantDto, int clanId)
    {
        var clan = HttpContext.GetClan();

        var entrant = clan.Entrants.FirstOrDefault(e => e.NormalizedGamertag == entrantDto.Gamertag.ToUpper());
        if (entrant != null) return Conflict(_mapper.Map<EntrantInfoDTO>(entrant));

        entrant = new Entrant
        {
            ClanId = clanId,
            Gamertag = entrantDto.Gamertag
        };
        
        clan.Entrants.Add(entrant);
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<EntrantInfoDTO>(entrant));
        
        return BadRequest();
    }
}