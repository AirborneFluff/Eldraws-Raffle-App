using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Data;
using RaffleApi.Extensions;
using RaffleApi.Results;

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
        var memberExists = await _unitOfWork.ClanRepository.IsUserMember((int)clanId, userId);
        
        if (!memberExists)
        {
            context.Result = new NotFoundObjectResult("Only clan members are authorized to do that");
            return;
        }
        
        await next.Invoke();
    }
}