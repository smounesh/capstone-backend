using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class ProfileViewRepository : IProfileViewRepository
    {
        private readonly JobPortalContext _context;

        public ProfileViewRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task LogProfileViewAsync(ProfileView profileView)
        {
            await _context.ProfileViews.AddAsync(profileView);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProfileView>> GetProfileViewsByProfileIdAsync(int profileId)
        {
            return await _context.ProfileViews
                .Include(pv => pv.Viewer)
                .Where(pv => pv.ProfileID == profileId)
                .ToListAsync();
        }
    }
}
