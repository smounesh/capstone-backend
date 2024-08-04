using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IProfileViewRepository
    {
        Task LogProfileViewAsync(ProfileView profileView);
        Task<IEnumerable<ProfileView>> GetProfileViewsByProfileIdAsync(int profileId);
    }
}
