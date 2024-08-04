using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/jobpostings")]
    [ApiController]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingService _jobPostingService;
        private readonly ILogger<JobPostingController> _logger;

        public JobPostingController(IJobPostingService jobPostingService, ILogger<JobPostingController> logger)
        {
            _jobPostingService = jobPostingService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobPostings = await _jobPostingService.GetAllJobPostingsAsync();
            return Ok(jobPostings);
        }

        [Authorize(Roles = "Admin,Employer")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobPostings()
        {
            var jobPostings = await _jobPostingService.GetAllJobPostingsAsync();
            return Ok(jobPostings);
        }

        //[Authorize(Roles = "Admin,Employer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobPostingById(int id)
        {
            var jobPosting = await _jobPostingService.GetJobPostingByIdAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            return Ok(jobPosting);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateJobPosting([FromBody] JobPostingCreateDto jobPostingCreateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdJobPosting = await _jobPostingService.CreateJobPostingAsync(jobPostingCreateDto, int.Parse(userId));
            return CreatedAtAction(nameof(GetJobPostingById), new { id = createdJobPosting.JobID }, createdJobPosting);
        }

        [Authorize(Roles = "Employer")]
        [HttpPut]
        public async Task<IActionResult> UpdateJobPosting([FromBody] JobPostingUpdateDto jobPostingUpdateDto)
        {
            _logger.LogDebug("Entering UpdateJobPosting method");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedJobPosting = await _jobPostingService.UpdateJobPostingAsync(jobPostingUpdateDto, int.Parse(userId));
            _logger.LogDebug("Exiting UpdateJobPosting method");
            return NoContent();
        }

        [Authorize(Roles = "Employer,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            await _jobPostingService.DeleteJobPostingAsync(id);
            return NoContent();
        }
    }
}
