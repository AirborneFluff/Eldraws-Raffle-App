using Microsoft.AspNetCore.Mvc;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StatusController : ControllerBase
{
    [HttpHead]
    public async Task<ActionResult> GetResponse()
    {
        return Ok();
    }
}