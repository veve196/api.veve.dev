using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using veve.Services;

namespace veve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TelegramController(TelegramService service) : Controller
    {
        [HttpGet("Stickers/{stickerSetName}")]
        public async Task<IActionResult> GetStickerSet(string stickerSetName)
        {
            string[] fileIds;

            try
            {
                fileIds = await service.GetStickerFileIds(stickerSetName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while processing your request."
                });
            }

            string baseUrl = $"https://{Request.Host}{Request.PathBase}";
            string[] stickerUrls = fileIds.Select(fileId => $"{baseUrl}/Telegram/Sticker/{fileId}").ToArray();

            return new JsonResult(stickerUrls);
        }

        [HttpGet("Sticker/{fileId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Sticker(string fileId)
        {
            try
            {
                var stickerUrl = await service.GetStickerUrl(fileId);
                using HttpClient httpClient = new();
                var response = await httpClient.GetAsync(stickerUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500, new ProblemDetails
                    {
                        Status = (int)response.StatusCode,
                        Title = "Failed to fetch the sticker image."
                    });
                }

                var imageStream = await response.Content.ReadAsStreamAsync();

                return File(imageStream, System.Net.Mime.MediaTypeNames.Image.Webp, $"{fileId}.webp");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while processing your request."
                });
            }

        }
    }
}