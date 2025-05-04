using Microsoft.AspNetCore.Mvc;

namespace veve.Controllers
{
    public class BaseController(IWebHostEnvironment hostEnvironment) : ControllerBase
    {
        protected IWebHostEnvironment WebHostEnvironment => hostEnvironment;

        [HttpGet("DateTime")]
        public IActionResult GetCurrentDateTime() => Ok(DateTime.Now);

        [HttpGet("UtcDateTime")]
        public IActionResult GetCurrentUtcDateTime() => Ok(DateTime.UtcNow);

        protected string GetErrorDetails(Exception ex)
        {
            return WebHostEnvironment.IsDevelopment() ? ex.Message : "Switch to development mode to see details.";
        }
    }
}
