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

public class RaffleController : BaseApiController
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

        var clan = await _unitOfWork.ClanRepository.GetById(raffleDto.ClanId);
        if (clan == null) return NotFound("No clan found by that Id");
        if (!clan.HasMember(userId)) return Unauthorized("You cannot add a raffle for this clan");

        var newRaffle = _mapper.Map<Raffle>(raffleDto);
        
        _unitOfWork.RaffleRepository.Add(newRaffle);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(newRaffle));

        return BadRequest();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateRaffle(int id, [FromBody] NewRaffleDTO raffleDto)
    {
        var userId = User.GetUserId();

        var raffle = await _unitOfWork.RaffleRepository.GetById(id);
        if (raffle == null) return NotFound("No raffle found by that Id");
        if (!raffle.HasMember(userId)) return Unauthorized("You cannot edit a raffle for this clan");

        _mapper.Map(raffleDto, raffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/entries")]
    public async Task<ActionResult> AddEntry(int raffleId, [FromBody] NewRaffleEntryDTO entryDto)
    {
        var userId = User.GetUserId();

        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("Raffle not found");
        if (!raffle.HasMember(userId)) return Unauthorized("You cannot edit a raffle for this clan");
        
        var newEntry = _mapper.Map<RaffleEntry>(entryDto);
        raffle.Entries.Add(newEntry);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
    
}