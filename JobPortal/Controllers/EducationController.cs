using JobPortal.Exceptions;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/educations")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<EducationController> _logger;

        public EducationController(IEducationService educationService, IProfileRepository profileRepository, ILogger<EducationController> logger)
        {
            _educationService = educationService;
            _profileRepository = profileRepository;
            _logger = logger;
        }

        private int GetProfileIdFromClaims()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Fetches the 'sub' claim
            var profile = _profileRepository.GetByUserIdAsync(int.Parse(userId)).Result;
            return profile?.ProfileID ?? 0;
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetEducations()
        {
            var profileId = GetProfileIdFromClaims();
            _logger.LogInformation("Fetching educations for profile ID: {ProfileId}", profileId);
            var educations = await _educationService.GetEducationsByProfileIdAsync(profileId);
            return Ok(educations);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEducation([FromBody] EducationCreateDto educationDto)
        {
            var profileId = GetProfileIdFromClaims();
            _logger.LogInformation("Creating education for profile ID: {ProfileId}", profileId);
            var createdEducation = await _educationService.CreateEducationAsync(profileId, educationDto);
            return CreatedAtAction(nameof(GetEducations), new { profileId }, createdEducation);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPut("{educationId}")]
        public async Task<IActionResult> UpdateEducation(int educationId, [FromBody] EducationUpdateDto educationDto)
        {
            var profileId = GetProfileIdFromClaims();
            _logger.LogInformation("Updating education ID: {EducationId} for profile ID: {ProfileId}", educationId, profileId);
            try
            {
                await _educationService.UpdateEducationAsync(educationId, educationDto);
                return NoContent();
            }
            catch (EducationNotFoundException)
            {
                _logger.LogWarning("Education ID: {EducationId} not found for profile ID: {ProfileId}", educationId, profileId);
                return NotFound(new { Message = "Education ID not found." });
            }
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpDelete("{educationId}")]
        public async Task<IActionResult> DeleteEducation(int educationId)
        {
            var profileId = GetProfileIdFromClaims();
            _logger.LogInformation("Deleting education ID: {EducationId} for profile ID: {ProfileId}", educationId, profileId);
            try
            {
                await _educationService.DeleteEducationAsync(profileId, educationId);
                return NoContent();
            }
            catch (EducationNotFoundException)
            {
                _logger.LogWarning("Education ID: {EducationId} not found for profile ID: {ProfileId}", educationId, profileId);
                return NotFound(new { Message = "Education ID not found." });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEducationsByUserId(int userId)
        {
            _logger.LogInformation("Fetching educations for user ID: {UserId}", userId);
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID: {UserId}", userId);
                return NotFound(new { Message = "Profile not found." });
            }

            var educations = await _educationService.GetEducationsByProfileIdAsync(profile.ProfileID);
            return Ok(educations);
        }
    }
}
