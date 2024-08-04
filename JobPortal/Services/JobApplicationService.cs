using System.Security.Claims;
using AutoMapper;
using JobPortal.Models.Dto;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using JobPortal.Exceptions;

namespace JobPortal.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IProfileRepository _profileRepository; // Assuming you have a ProfileRepository
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobApplicationService(
            IJobApplicationRepository jobApplicationRepository,
            IProfileRepository profileRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<JobApplicationDto>> GetAllAsync()
        {
            var jobApplications = await _jobApplicationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);
        }

        public async Task<JobApplicationDto> GetByIdAsync(int id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                throw new JobApplicationNotFoundException($"Job application with ID {id} not found.");
            }
            return _mapper.Map<JobApplicationDto>(jobApplication);
        }

        public async Task<IEnumerable<JobApplicationDto>> GetByUserIdAsync(int userId)
        {
            var jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);
        }

        public async Task<JobApplicationDto> AddAsync(JobApplicationCreateDto jobApplicationCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not authorized.");
            }

            var profile = await _profileRepository.GetByUserIdAsync(int.Parse(userId));
            if (profile == null)
            {
                throw new KeyNotFoundException("Profile not found for the user.");
            }

            var existingApplications = await _jobApplicationRepository.GetByUserIdAsync(int.Parse(userId));
            if (existingApplications.Any(app => app.JobPostingId == jobApplicationCreateDto.JobPostingId))
            {
                throw new DuplicateJobApplicationException("User has already submitted an application for this job posting.");
            }

            var jobApplication = _mapper.Map<JobApplication>(jobApplicationCreateDto);
            jobApplication.UserId = int.Parse(userId);
            jobApplication.username = profile.Name;
            jobApplication.Status = Enums.JobApplicationStatus.Submitted;
            var createdJobApplication = await _jobApplicationRepository.AddAsync(jobApplication);
            return _mapper.Map<JobApplicationDto>(createdJobApplication);
        }

        public async Task<JobApplicationDto> UpdateAsync(int id, JobApplicationUpdateDto jobApplicationUpdateDto)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                throw new JobApplicationNotFoundException($"Job application with ID {id} not found.");
            }

            _mapper.Map(jobApplicationUpdateDto, jobApplication);
            var updatedJobApplication = await _jobApplicationRepository.UpdateAsync(jobApplication);
            return _mapper.Map<JobApplicationDto>(updatedJobApplication);
        }

        public async Task DeleteAsync(int id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                throw new JobApplicationNotFoundException($"Job application with ID {id} not found.");
            }
            await _jobApplicationRepository.DeleteAsync(id);
        }
    }
}
