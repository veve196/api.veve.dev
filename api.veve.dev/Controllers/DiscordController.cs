using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using veve.Services;

namespace veve.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DiscordController(ILogger<DiscordController> logger, DiscordService service) : ControllerBase
{
    [HttpGet("/Users/{userId}")]
    public async Task<IActionResult> GetUser(ulong userId)
    {
        try
        {
            var user = await service.GetUserAsnyc(userId);
            return new JsonResult(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get Discord user with id {userId}", userId);
            return StatusCode(500, new ProblemDetails
            {
                Status = 500,
                Title = "An error occurred while processing your request.",
                Detail = ex.Message
            });
        }
    }

    [HttpGet("Users/{userId}/Spotify")]
    public async Task<IActionResult> GetSpotifyActivity(ulong userId)
    {
        try
        {
            var spotifyActivity = await service.GetSpotifyActivityAsync(userId);

            return new JsonResult(spotifyActivity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get Spotify activity for user with id {userId}", userId);
            return StatusCode(500, new ProblemDetails
            {
                Status = 500,
                Title = "An error occurred while processing your request.",
                Detail = ex.Message
            });
        }
    }
}
