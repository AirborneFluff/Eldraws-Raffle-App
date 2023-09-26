using RaffleApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RaffleApi.Data.DTOs;
using AutoMapper;
using RaffleApi.Services;
using RaffleApi.Extensions;

namespace RaffleApi.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("test")]
    public async Task<ActionResult<string>> Test()
    {
        var result = await _userManager.Users.FirstOrDefaultAsync();
        if (result == null) return NotFound("No users in the database");

        var testToken = await _tokenService.CreateToken(new AppUser());

        return Ok($"Username: {result.UserName}\nToken: {testToken}");
    }

    [HttpPost("login")]
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
    public async Task<ActionResult<AppUserDTO>> Register(RegisterDTO userDetails)
    {
        var newUser = new AppUser();
        newUser.UserName = userDetails.UserName;

        var result = await _userManager.CreateAsync(newUser, userDetails.Password);
        if (!result.Succeeded) return BadRequest("Couldn't add user");

        var userResult = _mapper.Map<AppUserDTO>(newUser);
        userResult.Token = await _tokenService.CreateToken(newUser);

        return userResult;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AppUserDTO>> GetUserInfo()
    {
        var userId = User.GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return NotFound();

        return _mapper.Map<AppUserDTO>(user);
    }
}