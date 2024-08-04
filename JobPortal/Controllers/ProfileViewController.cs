using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/profile")]
    [ApiController]
    public class ProfileViewController : ControllerBase
    {
        private readonly IProfileViewService _profileViewService;
        private readonly ILogger<ProfileViewController> _logger;

        public ProfileViewController(IProfileViewService profileViewService, ILogger<ProfileViewController> logger)
        {
            _profileViewService = profileViewService;
            _logger = logger;
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPost("{profileId}/views")]
        public async Task<IActionResult> LogProfileView(int profileId)
        {
            var viewerId = GetCurrentUserId(); // Method to get the authenticated user's ID
            _logger.LogInformation("User ID: {ViewerId} is viewing Profile ID: {ProfileId}", viewerId, profileId);

            await _profileViewService.LogProfileViewAsync(viewerId, profileId);
            return NoContent();
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpGet("{profileId}/views")]
        public async Task<IActionResult> GetProfileViews(int profileId)
        {
            _logger.LogInformation("Retrieving views for Profile ID: {ProfileId}", profileId);
            var profileViews = await _profileViewService.GetProfileViewsAsync(profileId);
            return Ok(profileViews);
        }


        private int GetCurrentUserId()
        {
            // Implement logic to get the user ID of the authenticated user
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
