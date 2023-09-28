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
        if (userId == null) return BadRequest();
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return BadRequest();

        var newRaffle = _mapper.Map<Raffle>(raffleDto);
        newRaffle.AppUserId = userId;
        
        _unitOfWork.RaffleRepository.Add(newRaffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(newRaffle));

        return BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditRaffle(int id, [FromBody] NewRaffleDTO raffleDto)
    {
        var userId = User.GetUserId();
        if (userId == null) return BadRequest();
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return BadRequest();
        
        var raffle = await _unitOfWork.RaffleRepository.GetById(id);
        if (raffle == null) return NotFound();

        if (raffle.AppUserId != userId) return Unauthorized();

        raffle.Title = raffleDto.Title;
        raffle.EntryCost = raffleDto.EntryCost;
        raffle.OpenDate = (DateTime) raffleDto.OpenDate;
        raffle.CloseDate = (DateTime) raffleDto.CloseDate;
        raffle.DrawDate = (DateTime) raffleDto.DrawDate;

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
}