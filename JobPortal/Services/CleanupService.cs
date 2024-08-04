using JobPortal.Contexts;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Services
{
    public class CleanupService
    {
        private readonly JobPortalContext _context;

        public CleanupService(JobPortalContext context)
        {
            _context = context;
        }

        public void CleanupDeletedProfiles()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);
            var profilesToDelete = _context.Profiles
                .Where(p => p.DeletedAt.HasValue && p.DeletedAt < cutoffDate)
                .ToList();

            _context.Profiles.RemoveRange(profilesToDelete);
            _context.SaveChanges();
        }
    }
}