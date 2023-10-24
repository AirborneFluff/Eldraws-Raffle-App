using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.ActionFilters;

public class ValidateUser : IAsyncActionFilter
{
    private readonly UserManager<AppUser> _userManager;

    public ValidateUser(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.GetUserId();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            context.Result = new UnauthorizedObjectResult("Issue validating your account");
            return;
        }
        
        context.HttpContext.Items.Add("user", user);
        await next.Invoke();
    }
}