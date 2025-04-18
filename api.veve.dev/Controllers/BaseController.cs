using Microsoft.AspNetCore.Mvc;

namespace veve.Controllers
{
    public class BaseController(IWebHostEnvironment hostEnvironment) : ControllerBase
    {
        protected IWebHostEnvironment WebHostEnvironment => hostEnvironment;

        protected string GetErrorDetails(Exception ex)
        {
            return WebHostEnvironment.IsDevelopment() ? ex.Message : "Switch to development mode to see details.";
        }
    }
}
