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
    [Route("api/v1/skills")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<SkillController> _logger;

        public SkillController(ISkillService skillService, IProfileRepository profileRepository, ILogger<SkillController> logger)
        {
            _skillService = skillService;
            _profileRepository = profileRepository;
            _logger = logger;
        }

        private async Task<int> GetProfileIdFromClaimsAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrEmpty(userId))
            {
                throw new System.InvalidOperationException("User ID claim is missing.");
            }
            var profile = await _profileRepository.GetByUserIdAsync(int.Parse(userId));
            return profile?.ProfileID ?? 0;
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            var profileId = await GetProfileIdFromClaimsAsync(User);
            _logger.LogInformation("Fetching skills for profile ID: {ProfileId}", profileId);
            var skills = await _skillService.GetSkillsByProfileIdAsync(profileId);
            return Ok(skills);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] SkillCreateDto skillDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(User);
            _logger.LogInformation("Creating skill for profile ID: {ProfileId}", profileId);
            var createdSkill = await _skillService.CreateSkillAsync(User, skillDto);
            return CreatedAtAction(nameof(GetSkills), new { profileId }, createdSkill);
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpPut("{skillId}")]
        public async Task<IActionResult> UpdateSkill(int skillId, [FromBody] SkillUpdateDto skillDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(User);
            skillDto.SkillID = skillId;

            // Check if the skill belongs to the logged-in user
            var skill = await _skillService.GetSkillByIdAsync(skillId);
            if (skill == null)
            {
                return NotFound(new { Message = $"Skill with ID {skillId} not found." });
            }
            if (skill.ProfileID != profileId)
            {
                throw new NotAuthorizedException("You are not authorized to update this skill.");
            }

            _logger.LogInformation("Updating skill ID: {SkillId} for profile ID: {ProfileId}", skillId, profileId);
            await _skillService.UpdateSkillAsync(User, skillDto);
            return NoContent();
        }

        [Authorize(Roles = "Applicant,Admin")]
        [HttpDelete("{skillId}")]
        public async Task<IActionResult> DeleteSkill(int skillId)
        {
            var profileId = await GetProfileIdFromClaimsAsync(User);

            // Check if the skill belongs to the logged-in user
            var skill = await _skillService.GetSkillByIdAsync(skillId);
            if (skill == null)
            {
                return NotFound(new { Message = $"Skill with ID {skillId} not found." });
            }
            if (skill.ProfileID != profileId)
            {
                throw new NotAuthorizedException("You are not authorized to delete this skill.");
            }

            _logger.LogInformation("Deleting skill ID: {SkillId} for profile ID: {ProfileId}", skillId, profileId);
            await _skillService.DeleteSkillAsync(User, skillId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSkillsByUserId(int userId)
        {
            _logger.LogInformation("Fetching skills for user ID: {UserId}", userId);
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID: {UserId}", userId);
                return NotFound(new { Message = "Profile not found." });
            }

            var skills = await _skillService.GetSkillsByProfileIdAsync(profile.ProfileID);
            if (skills == null)
            {
                _logger.LogWarning("Skills not found for user ID: {UserId}", userId);
                return NotFound(new { Message = "Skills not found." });
            }
            return Ok(skills);
        }
    }
}
