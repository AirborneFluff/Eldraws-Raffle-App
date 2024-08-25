using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Data;
using RaffleApi.Extensions;
using RaffleApi.Results;

namespace RaffleApi.ActionFilters;

public sealed class ValidateClanOwner : IAsyncActionFilter
{
    private readonly UnitOfWork _unitOfWork;

    public ValidateClanOwner(UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var clanId = (int?) context.ActionArguments["clanId"];
        if (clanId == null) throw new Exception("ClanId not provided for validation");
        
        var userId = context.HttpContext.User.GetUserId();
        var isOwner = await _unitOfWork.ClanRepository.IsUserOwner((int)clanId, userId);
        
        if (!isOwner)
        {
            context.Result = new NotFoundObjectResult("Only the clan owner is authorized to do that");
            return;
        }
        
        await next.Invoke();
    }
}