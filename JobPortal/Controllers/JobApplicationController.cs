using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("api/v1/jobapplication")]
    [ApiController]
    [Authorize] // Restrict all endpoints to authorized users
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAll()
        {
            var jobApplications = await _jobApplicationService.GetAllAsync();
            return Ok(jobApplications);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetById(int id)
        {
            var jobApplication = await _jobApplicationService.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            return Ok(jobApplication);
        }

        [HttpGet("getbyuserid/{userId}")]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetByUserId(int userId)
        {
            var jobApplications = await _jobApplicationService.GetByUserIdAsync(userId);
            return Ok(jobApplications);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Applicant, Admin")] // Restrict to Applicant and Admin roles
        public async Task<ActionResult<JobApplicationDto>> Create(JobApplicationCreateDto jobApplicationCreateDto)
        {
            var jobApplication = await _jobApplicationService.AddAsync(jobApplicationCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = jobApplication.Id }, jobApplication);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Applicant, Admin, Employer")] // Restrict to Applicant and Admin roles
        public async Task<IActionResult> Update(int id, JobApplicationUpdateDto jobApplicationUpdateDto)
        {
            var jobApplication = await _jobApplicationService.UpdateAsync(id, jobApplicationUpdateDto);
            if (jobApplication == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Applicant, Admin")] // Restrict to Applicant and Admin roles
        public async Task<IActionResult> Delete(int id)
        {
            await _jobApplicationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
