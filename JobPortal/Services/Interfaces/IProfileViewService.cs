using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IProfileViewService
    {
        Task LogProfileViewAsync(int viewerId, int profileId);
        Task<IEnumerable<ProfileViewDto>> GetProfileViewsAsync(int profileId);
    }
}
