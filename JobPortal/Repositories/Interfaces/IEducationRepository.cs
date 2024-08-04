using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IEducationRepository
    {
        Task<IEnumerable<Education>> GetByProfileIdAsync(int profileId);
        Task<Education> GetByIdAndProfileIdAsync(int educationId, int profileId);
        Task<Education> GetByIdAsync(int educationId);
        Task AddAsync(Education education);
        Task UpdateAsync(Education education);
        Task DeleteAsync(int educationId);
    }
}
