using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using veve.Services;

namespace veve.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DiscordController(ILogger<DiscordController> logger, DiscordService service) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(ulong userId)
    {
        try
        {
            var user = await service.GetUserAsnyc(userId);
            return new JsonResult(user);
        }
        catch (Exception ex)
        {
            string errorMsg = $"Failed to get Discord user with id {userId}";
            logger.LogError(ex, errorMsg);
            return StatusCode(500, new
            {
                error = errorMsg
            });
        }
    }

    [HttpGet("spotify/{userId}")]
    public async Task<IActionResult> GetSpotifyActivity(ulong userId)
    {
        {
            try
            {
                var spotifyActivity = await service.GetSpotifyActivityAsync(userId);

                return new JsonResult(spotifyActivity);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Failed to get Spotify status for user with id {userId}";
                logger.LogError(ex, errorMsg);
                return StatusCode(500, new
                {
                    error = errorMsg
                });
            }
        }
    }
}
