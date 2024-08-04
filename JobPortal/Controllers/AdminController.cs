using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("hangfire")]
        public IActionResult AccessHangfireDashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return new RedirectResult("/hangfire");
            }
            else
            {
                return Forbid();
            }
        }
    }
}
