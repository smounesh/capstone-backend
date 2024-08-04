using JobPortal.Enums;
using JobPortal.Models;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile?> GetByUserIdAsync(int userId);
        Task<Profile?> GetByIdAsync(int profileId);
        Task AddAsync(Profile profile);
        Task UpdateAsync(Profile profile);
        Task DeleteAsync(int profileId, DeletedBy deletedBy);
    }
}
