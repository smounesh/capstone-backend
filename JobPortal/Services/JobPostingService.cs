using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly ILogger<JobPostingService> _logger;
        private readonly IMapper _mapper;

        public JobPostingService(IJobPostingRepository jobPostingRepository, ILogger<JobPostingService> logger, IMapper mapper)
        {
            _jobPostingRepository = jobPostingRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobPostingDto>> GetAllJobPostingsAsync()
        {
            _logger.LogDebug("Retrieving all job postings.");
            var jobPostings = await _jobPostingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<JobPostingDto>>(jobPostings);
        }

        public async Task<JobPostingDto> GetJobPostingByIdAsync(int id)
        {
            _logger.LogDebug("Retrieving job posting by ID: {id}", id);
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            return _mapper.Map<JobPostingDto>(jobPosting);
        }

        public async Task<JobPostingDto> CreateJobPostingAsync(JobPostingCreateDto jobPostingCreateDto, int userId)
        {
            _logger.LogInformation("Creating a new job posting: {Title}", jobPostingCreateDto.Title);
            var jobPosting = _mapper.Map<JobPosting>(jobPostingCreateDto);
            jobPosting.PostedBy = userId;
            await _jobPostingRepository.AddAsync(jobPosting);
            return _mapper.Map<JobPostingDto>(jobPosting);
        }

        public async Task<JobPostingDto> UpdateJobPostingAsync(JobPostingUpdateDto jobPostingUpdateDto, int userId)
        {
            _logger.LogInformation("Updating job posting: {JobID}", jobPostingUpdateDto.JobID);
            var jobPosting = _mapper.Map<JobPosting>(jobPostingUpdateDto);
            jobPosting.PostedBy = userId;
            await _jobPostingRepository.UpdateAsync(jobPosting);
            return _mapper.Map<JobPostingDto>(jobPosting);
        }

        public async Task DeleteJobPostingAsync(int id)
        {
            _logger.LogInformation("Deleting job posting: {id}", id);
            await _jobPostingRepository.DeleteAsync(id);
        }
    }
}
