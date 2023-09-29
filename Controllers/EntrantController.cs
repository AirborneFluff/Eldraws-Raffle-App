using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Services;

namespace RaffleApi.Controllers;

public sealed class EntrantController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly UnitOfWork _unitOfWork;

    public EntrantController(UserManager<AppUser> userManager,
        TokenService tokenService, IMapper mapper, SignInManager<AppUser> signInManager, UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
        this._signInManager = signInManager;
        this._userManager = userManager;
        this._mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<ActionResult<List<Entrant>>> AddNewEntrant([FromBody] NewEntrantDTO entrantDto)
    {
        var clan = await _unitOfWork.ClanRepository.GetById(entrantDto.ClanId);
        if (clan == null) return NotFound("No clan found by that Id");

        var entrant = await _unitOfWork.EntrantRepository.GetByGamertag(entrantDto.Gamertag);
        if (entrant != null) return Conflict("That entrant already exists for this clan");

        var newEntrant = _mapper.Map<Entrant>(entrantDto);

        _unitOfWork.EntrantRepository.Add(newEntrant);

        if (await _unitOfWork.Complete()) return Ok(newEntrant);

        return BadRequest();
    }
}