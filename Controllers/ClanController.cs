using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ClanController : ControllerBase
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
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ClanDTO>> CreateNewClan(ClanDTO clan)
    {
        var userId = User.GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return BadRequest("Invalid login credentials");

        var currentClan = await _unitOfWork.ClanRepository.GetByName(clan.Name);
        if (currentClan != null) return Conflict("This clan name is already in use");

        var newClan = _mapper.Map<Clan>(clan);
        user.UserClans.Add(newClan);

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) return BadRequest("Issue creating new clan");

        return _mapper.Map<ClanDTO>(newClan);
    }
}