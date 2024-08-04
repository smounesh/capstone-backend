using JobPortal.Contexts;
using JobPortal.Enums;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly JobPortalContext _context;

        public ProfileRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<Profile?> GetByUserIdAsync(int userId)
        {
            return await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserID == userId);
        }

        public async Task<Profile?> GetByIdAsync(int profileId)
        {
            return await _context.Profiles.FindAsync(profileId);
        }

        public async Task AddAsync(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int profileId, DeletedBy deletedBy)
        {
            var profile = await GetByIdAsync(profileId);
            if (profile != null)
            {
                profile.DeletedAt = DateTime.UtcNow; 
                profile.DeletedBy = deletedBy; 
                await UpdateAsync(profile);
            }
        }
    }
}
