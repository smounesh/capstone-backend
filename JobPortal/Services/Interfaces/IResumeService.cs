using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeDto> GetResumeByIdAsync(int resumeId);
        Task<IEnumerable<ResumeDto>> GetResumesByUserIdAsync(int userId);
        Task<ResumeDto> CreateResumeAsync(ResumeCreateDto resumeDto);
        Task<ResumeDto> UpdateResumeAsync(int resumeId, ResumeUpdateDto resumeDto);
        Task DeleteResumeAsync(int resumeId);
    }
}
