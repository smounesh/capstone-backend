using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortal.Models;

namespace JobPortal.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();
        Task<JobApplication> GetByIdAsync(int id);
        Task<IEnumerable<JobApplication>> GetByUserIdAsync(int userId); 
        Task<JobApplication> AddAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateAsync(JobApplication jobApplication);
        Task DeleteAsync(int id);
    }
}
