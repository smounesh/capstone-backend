using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IEducationService
    {
        Task<IEnumerable<EducationDto>> GetEducationsByProfileIdAsync(int profileId);
        Task<EducationDto> CreateEducationAsync(int profileId, EducationCreateDto educationDto);
        Task<EducationDto> UpdateEducationAsync(int educationId, EducationUpdateDto educationDto);
        Task DeleteEducationAsync(int profileId, int educationId);
    }
}
