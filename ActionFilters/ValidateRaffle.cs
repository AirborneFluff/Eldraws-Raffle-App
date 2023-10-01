using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Data;

namespace RaffleApi.ActionFilters;

public class ValidateRaffle : IAsyncActionFilter
{
    private readonly UnitOfWork _unitOfWork;

    public ValidateRaffle(UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var raffleId = (int?) context.ActionArguments["raffleId"];
        if (raffleId == null) throw new Exception("RaffleId not provided for validation");
        
        var raffle = await _unitOfWork.RaffleRepository.GetById((int) raffleId);
        
        if (raffle == null)
        {
            context.Result = new NotFoundObjectResult("No raffle found by that Id");
            return;
        }
        
        context.HttpContext.Items.Add("raffle", raffle);
        await next.Invoke();
    }
}