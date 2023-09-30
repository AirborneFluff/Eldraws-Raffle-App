using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Controllers;

public sealed class ClanController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public ClanController(IMapper mapper, UnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    
    [HttpPost]
    public async Task<ActionResult<ClanDTO>> CreateNewClan(NewClanDTO clan)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest();
        
        var newClan = _mapper.Map<Clan>(clan);
        
        newClan.OwnerId = userId;
        
        _unitOfWork.ClanRepository.Add(newClan);
        if (!await _unitOfWork.Complete()) return BadRequest("Issue creating new clan");

        var clanMember = new ClanMember
        {
            MemberId = userId,
            ClanId = newClan.Id
        };
        newClan.Members.Add(clanMember);
        
        if (!await _unitOfWork.Complete()) return BadRequest("Issue adding member to clan");
        
        return Ok(_mapper.Map<ClanDTO>(newClan));
    }
    
    [HttpPost("{clanId:int}/members/{memberId}")]
    public async Task<ActionResult<ClanDTO>> AddMember(string memberId, int clanId)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var clan = await _unitOfWork.ClanRepository.GetById(clanId);
        if (clan == null) return NotFound("No clan found by that Id");

        if (clan.OwnerId != userId) return Unauthorized("Only the clan owner can add members");

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
    public async Task<ActionResult<ClanDTO>> RemoveMember(string memberId, int clanId)
    {
        var userId = User.GetUserId();
        if (memberId == userId) return BadRequest("You cannot remove yourself from the clan");
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var clan = await _unitOfWork.ClanRepository.GetById(clanId);
        if (clan == null) return NotFound("No clan found by that Id");

        if (clan.OwnerId != userId) return Unauthorized("Only the clan owner can remove members");

        var clanMember = clan.Members.FirstOrDefault(m => m.MemberId == memberId);
        if (clanMember == null) return NotFound("That user is not a member of this clan");
        
        clan.Members.Remove(clanMember);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<ClanDTO>(clan));
        
        return BadRequest();
    }
}