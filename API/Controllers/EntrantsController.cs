using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Extensions;
using RaffleApi.Helpers;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/clans/{clanId}/entrants")]
[Authorize]
[ServiceFilter(typeof(ValidateUser))]
[ServiceFilter(typeof(ValidateClanMember))]
public sealed class EntrantsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public EntrantsController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult> GetPaginatedEntrants([FromQuery] EntrantParams entrantParams, int clanId)
    {
        var result = await _unitOfWork.EntrantRepository.GetByClan(entrantParams, clanId);
        var entrants =  result.Select(entrant => _mapper.Map<EntrantDTO>(entrant));
        
        Response.AddPaginationHeader(result);
        return Ok(entrants);
    }

    [HttpPost("{entrantId:int}/setInactive")]
    public async Task<ActionResult> SetEntrantInactive(int clanId, int entrantId)
    {
        var entrant = await _unitOfWork.EntrantRepository.GetById(entrantId);
        if (entrant == null) return NotFound();

        entrant.Active = false;
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<EntrantDTO>(entrant));
        return BadRequest();
    }

    [HttpPost("{entrantId:int}/setActive")]
    public async Task<ActionResult> SetEntrantActive(int clanId, int entrantId)
    {
        var entrant = await _unitOfWork.EntrantRepository.GetById(entrantId);
        if (entrant == null) return NotFound();

        entrant.Active = true;
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<EntrantDTO>(entrant));
        return BadRequest();
    }
}