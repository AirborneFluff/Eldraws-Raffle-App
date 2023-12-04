﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;
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
    public async Task<ActionResult> GetPaginatedEntrants([FromQuery] EntrantParams entrantParams, int clanId)
    {
        var result = await _unitOfWork.EntrantRepository.GetByClan(entrantParams, clanId);
        var entrants =  result.Select(entrant => _mapper.Map<EntrantDTO>(entrant));
        
        Response.AddPaginationHeader(result);
        return Ok(entrants);
    }
}