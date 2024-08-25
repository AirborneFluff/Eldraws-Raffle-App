using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaffleApi.Data;

namespace RaffleApi.ActionFilters;

public class ValidateRaffleExists : IAsyncActionFilter
{
    private readonly UnitOfWork _unitOfWork;

    public ValidateRaffleExists(UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var raffleId = (int?) context.ActionArguments["raffleId"];
        if (raffleId == null) throw new Exception("RaffleId not provided for validation");
        
        var raffleExists = await _unitOfWork.RaffleRepository.Exists((int) raffleId);
        
        if (!raffleExists)
        {
            context.Result = new NotFoundObjectResult("No raffle found by that Id");
            return;
        }
        
        await next.Invoke();
    }
}