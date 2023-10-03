using IGBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace IGBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotController : ControllerBase
{
    private readonly IBotIGService _updateService;

    public BotController(IBotIGService updateService)
    {
        _updateService = updateService;
    }

    [HttpPost]
    public async Task<IActionResult> HandleUpdate([FromBody] Update update,
        CancellationToken cancellationToken = default)
    {
        await _updateService.HandleUpdate(update, cancellationToken);
        return Ok();
    }
}