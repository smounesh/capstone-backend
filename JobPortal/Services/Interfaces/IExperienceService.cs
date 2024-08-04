using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<IEnumerable<ExperienceDto>> GetExperiencesByUserIdAsync(int userId);
        Task<ExperienceDto> CreateExperienceAsync(ClaimsPrincipal user, ExperienceCreateDto experienceDto);
        Task<ExperienceDto> UpdateExperienceAsync(ClaimsPrincipal user, ExperienceUpdateDto experienceDto);
        Task DeleteExperienceAsync(ClaimsPrincipal user, int experienceId);
    }
}
