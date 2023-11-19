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

        var clan = await _unitOfWork.ClanRepository.GetById((int) clanId);
        if (clan == null)
        {
            context.Result = new NotFoundObjectResult("No clan found by that Id");
            return;
        }
        if (clan.OwnerId == userId)
        {
            context.HttpContext.Items.Add("clan", clan);
            await next.Invoke();
            return;
        }

        context.Result = new ForbiddenObjectResult("Only the clan owner is authorized to do that");
    }
}