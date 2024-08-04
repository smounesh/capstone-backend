using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume> GetResumeByIdAsync(int resumeId);
        Task<IEnumerable<Resume>> GetResumesByUserIdAsync(int userId);
        Task AddResumeAsync(Resume resume);
        Task UpdateResumeAsync(Resume resume);
        Task DeleteResumeAsync(Resume resume);
    }
}
