using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/resumes")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(IResumeService resumeService, ILogger<ResumeController> logger)
        {
            _resumeService = resumeService;
            _logger = logger;
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public async Task<IActionResult> GetResumesByUserId()
        {
            var userId = GetCurrentUserId(); // Method to get the authenticated user's ID
            var resumes = await _resumeService.GetResumesByUserIdAsync(userId);
            return Ok(resumes);
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet("{resumeId}")]
        public async Task<IActionResult> GetResumeById(int resumeId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            return Ok(resume);
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromForm] ResumeCreateDto resumeDto)
        {
            if (!ModelState.IsValid || resumeDto.File == null || resumeDto.File.Length == 0)
            {
                return BadRequest(ModelState);
            }

            // Extract the original file name from the IFormFile
            string originalFileName = resumeDto.File.FileName;
            _logger.LogInformation($"Original file name: {originalFileName}");

            // Create the resume using the file stream and original file name
            var createdResume = await _resumeService.CreateResumeAsync(resumeDto);
            return CreatedAtAction(nameof(GetResumeById), new { resumeId = createdResume.ResumeID }, createdResume);
        }

        [Authorize(Roles = "Applicant")]
        [HttpPut("{resumeId}")]
        public async Task<IActionResult> UpdateResume(int resumeId, [FromForm] ResumeUpdateDto resumeDto)
        {
            if (resumeId != resumeDto.ResumeID || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedResume = await _resumeService.UpdateResumeAsync(resumeId, resumeDto);
            return Ok(updatedResume);
        }

        [Authorize(Roles = "Applicant")]
        [HttpDelete("{resumeId}")]
        public async Task<IActionResult> DeleteResume(int resumeId)
        {
            await _resumeService.DeleteResumeAsync(resumeId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetResumesByUserId(int userId)
        {
            _logger.LogInformation("Fetching resumes for user ID: {UserId}", userId);
            var resumes = await _resumeService.GetResumesByUserIdAsync(userId);
            if (resumes == null)
            {
                _logger.LogWarning("Resumes not found for user ID: {UserId}", userId);
                return NotFound(new { Message = "Resumes not found." });
            }
            return Ok(resumes);
        }

        private int GetCurrentUserId()
        {
            // Implement logic to get the userId of the authenticated user
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID claim not found");
            }
            return int.Parse(userIdClaim);
        }
    }
}
