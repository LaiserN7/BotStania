using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateController : ControllerBase
{
    private ILogger<UpdateController> Logger { get; }
    private IUpdateService UpdateService { get; }

    public UpdateController(ILogger<UpdateController> logger, IUpdateService updateService) =>
        (Logger, UpdateService) = (logger, updateService);

    [HttpPost]
    public async Task<IActionResult> Post(string json)
    {
        await UpdateService.EchoAsync(json);
        return Ok();
    }
}

