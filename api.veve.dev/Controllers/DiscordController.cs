using Microsoft.AspNetCore.Mvc;
using veve.Services;

namespace veve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscordController(DiscordService service) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserStatus(ulong userId)
        {
            var user = await service.Client.GetUserAsync(userId);
            return new JsonResult(user);
        }
    }
}
