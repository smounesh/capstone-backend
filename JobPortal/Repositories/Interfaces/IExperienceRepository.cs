using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IExperienceRepository
    {
        Task<IEnumerable<Experience>> GetByProfileIdAsync(int profileId);
        Task<Experience> GetByIdAsync(int experienceId); 
        Task AddAsync(Experience experience);
        Task UpdateAsync(Experience experience);
        Task DeleteAsync(int experienceId);
    }
}
