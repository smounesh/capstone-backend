using JobPortal.Models.Dto;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IJobPostingService
    {
        Task<IEnumerable<JobPostingDto>> GetAllJobPostingsAsync();
        Task<JobPostingDto> GetJobPostingByIdAsync(int id);
        Task<JobPostingDto> CreateJobPostingAsync(JobPostingCreateDto jobPostingCreateDto, int userID);
        Task<JobPostingDto> UpdateJobPostingAsync(JobPostingUpdateDto jobPostingUpdateDto, int userID);
        Task DeleteJobPostingAsync(int id);
    }
}

