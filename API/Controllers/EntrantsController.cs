using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/clans/{clanId}/entrants")]
[ServiceFilter(typeof(ValidateUser))]
[ServiceFilter(typeof(ValidateClanMember))]
public sealed class EntrantsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public EntrantsController(IMapper mapper, UnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult> GetPaginatedEntrants([FromQuery] PaginationParams pagination, int clanId)
    {
        var result = await _unitOfWork.EntrantRepository.GetByClan(pagination, clanId);
        var entrants =  result.Select(entrant => _mapper.Map<EntrantDTO>(entrant));
        
        return Ok(entrants);
    }
}