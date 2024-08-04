using JobPortal.Models;
using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillDto>> GetSkillsByProfileIdAsync(int profileId);
        Task<SkillDto> CreateSkillAsync(ClaimsPrincipal user, SkillCreateDto skillDto);
        Task<SkillDto> UpdateSkillAsync(ClaimsPrincipal user, SkillUpdateDto skillDto);
        Task DeleteSkillAsync(ClaimsPrincipal user, int skillId);
        Task<Skill> GetSkillByIdAsync(int skillId);
    }
}
