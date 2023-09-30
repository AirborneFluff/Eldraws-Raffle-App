using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using RaffleApi.Services;

namespace RaffleApi.Controllers;

public sealed class RaffleController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UnitOfWork _unitOfWork;

    public RaffleController(UserManager<AppUser> userManager,
        TokenService tokenService, IMapper mapper, SignInManager<AppUser> signInManager,
        UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
        this._signInManager = signInManager;
        this._mapper = mapper;
        this._tokenService = tokenService;
        this._userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult> CreateNewRaffle([FromBody] NewRaffleDTO raffleDto)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var clan = await _unitOfWork.ClanRepository.GetById(raffleDto.ClanId);
        if (clan == null) return NotFound("No clan found by that Id");
        
        if (!clan.HasMember(userId)) return Unauthorized("Only members of the clan can create raffles");

        var newRaffle = _mapper.Map<Raffle>(raffleDto);
        newRaffle.HostId = userId;
        
        _unitOfWork.RaffleRepository.Add(newRaffle);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(newRaffle));

        return BadRequest("Issue creating raffle");
    }

    [HttpPut("{raffleId:int}")]
    public async Task<ActionResult> UpdateRaffle(int raffleId, [FromBody] NewRaffleDTO raffleDto)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("No raffle found by that Id");
        
        if (!raffle.Clan!.HasMember(userId)) return Unauthorized("You cannot edit a raffle for this clan");

        _mapper.Map(raffleDto, raffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/entries")]
    public async Task<ActionResult> AddEntry(int raffleId, [FromBody] NewRaffleEntryDTO entryDto)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("Raffle not found");
        
        if (!raffle.Clan!.HasMember(userId)) return Unauthorized("You cannot edit a raffle for this clan");

        var entrants = await _unitOfWork.EntrantRepository.GetAllByClan(raffle.ClanId);
        var entrant = entrants.FirstOrDefault(e => e.Id == entryDto.EntrantId);
        if (entrant == null) return NotFound("No entrant found by that Id in this clan");
        
        var newEntry = _mapper.Map<RaffleEntry>(entryDto);
        raffle.Entries.Add(newEntry);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/entries/{entryId:int}")]
    public async Task<ActionResult> RemoveEntry(int raffleId, int entryId)
    {
        var userId = User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Issue validating your account");

        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("Raffle not found");
        
        if (!raffle.Clan!.HasMember(userId)) return Unauthorized("You cannot edit a raffle for this clan");

        var entry = raffle.Entries.FirstOrDefault(e => e.Id == entryId);
        if (entry == null) return NotFound("No entry found by that Id");
        
        raffle.Entries.Remove(entry);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
    
}