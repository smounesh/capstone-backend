using JobPortal.Contexts;
using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly JobPortalContext _context;
        private readonly ILogger<ExperienceRepository> _logger;

        public ExperienceRepository(JobPortalContext context, ILogger<ExperienceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Experience>> GetByProfileIdAsync(int profileId)
        {
            _logger.LogInformation("Fetching experiences for profile ID: {ProfileId}", profileId);
            return await _context.Experiences.Where(e => e.ProfileID == profileId).ToListAsync();
        }

        public async Task<Experience> GetByIdAsync(int experienceId)
        {
            _logger.LogInformation("Fetching experience ID: {ExperienceId}", experienceId);
            return await _context.Experiences.FindAsync(experienceId);
        }

        public async Task AddAsync(Experience experience)
        {
            _logger.LogInformation("Adding new experience for profile ID: {ProfileId}", experience.ProfileID);
            await _context.Experiences.AddAsync(experience);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Experience added successfully with ID: {ExperienceId}", experience.ExperienceID);
        }

        public async Task UpdateAsync(Experience experience)
        {
            _logger.LogInformation("Updating experience ID: {ExperienceId}", experience.ExperienceID);
            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Experience updated successfully.");
        }

        public async Task DeleteAsync(int experienceId)
        {
            _logger.LogInformation("Deleting experience ID: {ExperienceId}", experienceId);
            var experience = await _context.Experiences.FindAsync(experienceId);
            if (experience != null)
            {
                _context.Experiences.Remove(experience);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Experience deleted successfully.");
            }
            else
            {
                _logger.LogWarning("Experience ID: {ExperienceId} not found for deletion.", experienceId);
                throw new ExperienceNotFoundException(experienceId);
            }
        }
    }
}
