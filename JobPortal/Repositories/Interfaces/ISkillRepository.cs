using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetByProfileIdAsync(int profileId);
        Task<Skill?> GetByIdAsync(int skillId);
        Task AddAsync(Skill skill);
        Task UpdateAsync(Skill skill);
        Task DeleteAsync(int skillId);
    }
}
