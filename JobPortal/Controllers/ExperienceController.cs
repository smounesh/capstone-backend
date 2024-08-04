using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/experiences")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(IExperienceService experienceService, ILogger<ExperienceController> logger)
        {
            _experienceService = experienceService;
            _logger = logger;
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetExperiences()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("Fetching experiences for user ID: {UserId}", userId);
            var experiences = await _experienceService.GetExperiencesByUserIdAsync(userId);
            return Ok(experiences);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceCreateDto experienceDto)
        {
            var user = User;
            _logger.LogInformation("Creating experience for user ID: {UserId}", user.FindFirstValue(ClaimTypes.NameIdentifier));
            var createdExperience = await _experienceService.CreateExperienceAsync(user, experienceDto);
            return CreatedAtAction(nameof(GetExperiences), new { profileId = createdExperience.ProfileID }, createdExperience);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPut("{experienceId}")]
        public async Task<IActionResult> UpdateExperience(int experienceId, [FromBody] ExperienceUpdateDto experienceDto)
        {
            var user = User;
            experienceDto.ExperienceID = experienceId;
            _logger.LogInformation("Updating experience ID: {ExperienceId} for user ID: {UserId}", experienceId, user.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                await _experienceService.UpdateExperienceAsync(user, experienceDto);
                return NoContent();
            }
            catch (ExperienceNotFoundException)
            {
                _logger.LogWarning("Experience ID: {ExperienceId} not found for user ID: {UserId}", experienceId, user.FindFirstValue(ClaimTypes.NameIdentifier));
                return NotFound(new { Message = "Experience ID not found." });
            }
            catch (NotAuthorizedException)
            {
                _logger.LogWarning("User ID: {UserId} is not authorized to update experience ID: {ExperienceId}", user.FindFirstValue(ClaimTypes.NameIdentifier), experienceId);
                return Forbid();
            }
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpDelete("{experienceId}")]
        public async Task<IActionResult> DeleteExperience(int experienceId)
        {
            var user = User;
            _logger.LogInformation("Deleting experience ID: {ExperienceId} for user ID: {UserId}", experienceId, user.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                await _experienceService.DeleteExperienceAsync(user, experienceId);
                return NoContent();
            }
            catch (ExperienceNotFoundException)
            {
                _logger.LogWarning("Experience ID: {ExperienceId} not found for user ID: {UserId}", experienceId, user.FindFirstValue(ClaimTypes.NameIdentifier));
                return NotFound(new { Message = "Experience ID not found." });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetExperiencesByUserId(int userId)
        {
            _logger.LogInformation("Fetching experiences for user ID: {UserId}", userId);
            var experiences = await _experienceService.GetExperiencesByUserIdAsync(userId);
            if (experiences == null)
            {
                _logger.LogWarning("Experiences not found for user ID: {UserId}", userId);
                return NotFound(new { Message = "Experiences not found." });
            }
            return Ok(experiences);
        }
    }
}
