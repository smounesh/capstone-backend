using JobPortal.Models.Dto;

namespace JobPortal.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplicationDto>> GetAllAsync();
        Task<JobApplicationDto> GetByIdAsync(int id);
        Task<IEnumerable<JobApplicationDto>> GetByUserIdAsync(int userId);
        Task<JobApplicationDto> AddAsync(JobApplicationCreateDto jobApplicationCreateDto);
        Task<JobApplicationDto> UpdateAsync(int id, JobApplicationUpdateDto jobApplicationUpdateDto);
        Task DeleteAsync(int id);
    }
}
