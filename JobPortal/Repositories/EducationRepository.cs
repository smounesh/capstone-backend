using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly JobPortalContext _context;

        public EducationRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Education>> GetByProfileIdAsync(int profileId)
        {
            return await _context.Educations
                .AsNoTracking()
                .Where(e => e.ProfileID == profileId)
                .ToListAsync();
        }

        public async Task<Education> GetByIdAndProfileIdAsync(int educationId, int profileId)
        {
            return await _context.Educations
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EducationID == educationId && e.ProfileID == profileId);
        }

        public async Task<Education> GetByIdAsync(int educationId)
        {
            return await _context.Educations.FindAsync(educationId);
        }

        public async Task AddAsync(Education education)
        {
            await _context.Educations.AddAsync(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Education education)
        {
            _context.Educations.Attach(education);
            _context.Entry(education).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int educationId)
        {
            var education = await _context.Educations.FindAsync(educationId);
            if (education != null)
            {
                _context.Educations.Remove(education);
                await _context.SaveChangesAsync();
            }
        }
    }
}
