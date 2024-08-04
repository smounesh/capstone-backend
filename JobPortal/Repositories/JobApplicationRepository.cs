using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly JobPortalContext _context;

        public JobApplicationRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await _context.JobApplications.ToListAsync();
        }

        public async Task<JobApplication> GetByIdAsync(int id)
        {
            return await _context.JobApplications.FindAsync(id);
        }

        public async Task<IEnumerable<JobApplication>> GetByUserIdAsync(int userId) // Add this method
        {
            return await _context.JobApplications.Where(ja => ja.UserId == userId).ToListAsync();
        }

        public async Task<JobApplication> AddAsync(JobApplication jobApplication)
        {
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task<JobApplication> UpdateAsync(JobApplication jobApplication)
        {
            _context.Entry(jobApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task DeleteAsync(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
                await _context.SaveChangesAsync();
            }
        }
    }
}
