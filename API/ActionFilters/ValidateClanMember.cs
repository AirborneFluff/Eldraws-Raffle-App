using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Data;
using RaffleApi.Extensions;

namespace RaffleApi.ActionFilters;

public class ValidateClanMember : IAsyncActionFilter
{
    private readonly UnitOfWork _unitOfWork;

    public ValidateClanMember(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var clanId = (int?) context.ActionArguments["clanId"];
        if (clanId == null) throw new Exception("ClanId not provided for validation");
        
        var userId = context.HttpContext.User.GetUserId();

        var clan = await _unitOfWork.ClanRepository.GetById((int) clanId);
        if (clan == null)
        {
            context.Result = new NotFoundObjectResult("No clan found by that Id");
            return;
        }
        if (clan.Members.FirstOrDefault(x => x.MemberId == userId) != null)
        {
            context.HttpContext.Items.Add("clan", clan);
            await next.Invoke();
            return;
        };

        context.Result = new UnauthorizedObjectResult("Only clan members are authorized to do that");
    }
}