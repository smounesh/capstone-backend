using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly JobPortalContext _context;
        private readonly DbSet<JobPosting> _jobPostings;

        public JobPostingRepository(JobPortalContext context)
        {
            _context = context;
            _jobPostings = context.Set<JobPosting>();
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            return await _jobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetByIdAsync(int id)
        {
            return await _jobPostings.FindAsync(id);
        }

        public async Task AddAsync(JobPosting jobPosting)
        {
            await _jobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobPosting jobPosting)
        {
            _jobPostings.Update(jobPosting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobPosting = await GetByIdAsync(id);
            if (jobPosting != null)
            {
                _jobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync();
            }
        }
    }
}
