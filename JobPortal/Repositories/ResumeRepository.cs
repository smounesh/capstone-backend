using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly JobPortalContext _context;

        public ResumeRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resume>> GetResumesByUserIdAsync(int userId)
        {
            return await _context.Resumes
                .Where(r => r.UserID == userId)
                .ToListAsync();
        }

        public async Task<Resume> GetResumeByIdAsync(int resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
            {
                throw new System.InvalidOperationException($"Resume with ID {resumeId} not found.");
            }
            return resume;
        }

        public async Task AddResumeAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResumeAsync(Resume resume)
        {
            _context.Resumes.Update(resume);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResumeAsync(Resume resume)
        {
            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
        }
    }
}
