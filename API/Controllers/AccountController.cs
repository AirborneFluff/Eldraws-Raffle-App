using RaffleApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RaffleApi.Data.DTOs;
using AutoMapper;
using RaffleApi.ActionFilters;
using RaffleApi.Services;
using RaffleApi.Extensions;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAllUsers()
    {
        var result = await _userManager.Users
            .Select(user => _mapper.Map<MemberDTO>(user))
            .ToListAsync();
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AppUserDTO>> Login(LoginDTO input)
    {
        if (input.UserName == null) return BadRequest("Please provide a Username");
        if (input.Password == null) return BadRequest("Please provide a password");

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper() == input.UserName.ToUpper());

        if (user == null) return NotFound("No account found with this email/password combination");

        var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);

        if (!result.Succeeded) return NotFound("No account found with this email/password combination");

        var userResult = _mapper.Map<AppUserDTO>(user);
        userResult.Token = await _tokenService.CreateToken(user);

        return userResult;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AppUserDTO>> Register(RegisterDTO userDetails)
    {
        var newUser = new AppUser();
        newUser.UserName = userDetails.UserName;

        var result = await _userManager.CreateAsync(newUser, userDetails.Password);
        if (!result.Succeeded)
        {
            var errorMsg = result.Errors.FirstOrDefault()?.Description;
            return BadRequest(errorMsg ?? "Issue creating user");
        }

        var userResult = _mapper.Map<AppUserDTO>(newUser);
        userResult.Token = await _tokenService.CreateToken(newUser);

        return userResult;
    }
    
    [HttpGet]
    [Authorize]
    [ServiceFilter(typeof(ValidateUser))]
    public ActionResult<AppUserDTO> GetUserInfo()
    {
        return Ok(_mapper.Map<AppUserDTO>(HttpContext.GetUser()));
    }
}