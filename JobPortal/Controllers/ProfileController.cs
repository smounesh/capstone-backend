using JobPortal.Enums;
using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(IProfileService profileService, IHttpContextAccessor httpContextAccessor)
        {
            _profileService = profileService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            var profileDto = await _profileService.GetProfileByUserIdAsync(userId);
            if (profileDto == null)
            {
                return NotFound();
            }
            return Ok(profileDto);
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProfileByUserId(int userId)
        {
            var profileDto = await _profileService.GetProfileByUserIdAsync(userId);
            if (profileDto == null)
            {
                return NotFound();
            }
            return Ok(profileDto);
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromForm] ProfileCreateDto profileCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var result = await _profileService.CreateProfileAsync(userId, profileCreateDto);
            if (result.IsConflict)
            {
                return Conflict(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetMyProfile), new { id = result.Profile.ProfileID }, result.Profile);
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromForm] ProfileUpdateDto profileUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var updatedProfile = await _profileService.UpdateProfileAsync(userId, profileUpdateDto);
            return Ok(updatedProfile);
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var userId = GetCurrentUserId();
            var deletedBy = User.IsInRole("Admin") ? DeletedBy.Admin : DeletedBy.User;
            await _profileService.DeleteProfileAsync(userId, deletedBy);
            return NoContent();
        }

        [Authorize]
        [HttpGet("userhasaprofile")]
        public async Task<IActionResult> UserHasAProfile()
        {
            var userId = GetCurrentUserId();
            var hasProfile = await _profileService.UserHasProfileAsync(userId);
            return Ok(new { HasProfile = hasProfile });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID claim not found");
            }
            return int.Parse(userIdClaim);
        }
    }
}
