using Microsoft.AspNetCore.Mvc;
using veve.Services;

namespace veve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifyController(ILogger<SpotifyController> logger, SpotifyService service) : ControllerBase
    {
        [HttpGet]
        [Route("Status")]
        public async Task<IActionResult> GetStatus()
        {
            throw new NotImplementedException();
        }

    }
}
