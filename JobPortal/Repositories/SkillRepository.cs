using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly JobPortalContext _context;
        private readonly ILogger<SkillRepository> _logger;

        public SkillRepository(JobPortalContext context, ILogger<SkillRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Skill>> GetByProfileIdAsync(int profileId)
        {
            _logger.LogInformation("Fetching skills for profile ID: {ProfileId}", profileId);
            return await _context.Skills.Where(s => s.ProfileID == profileId).ToListAsync();
        }

        public async Task<Skill?> GetByIdAsync(int skillId)
        {
            _logger.LogInformation("Fetching skill by ID: {SkillId}", skillId);
            return await _context.Skills.FindAsync(skillId);
        }

        public async Task AddAsync(Skill skill)
        {
            _logger.LogInformation("Adding new skill for profile ID: {ProfileId}", skill.ProfileID);
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Skill added successfully with ID: {SkillId}", skill.SkillID);
        }

        public async Task UpdateAsync(Skill skill)
        {
            _logger.LogInformation("Updating skill ID: {SkillId}", skill.SkillID);
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Skill updated successfully.");
        }

        public async Task DeleteAsync(int skillId)
        {
            _logger.LogInformation("Deleting skill ID: {SkillId}", skillId);
            var skill = await _context.Skills.FindAsync(skillId);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Skill deleted successfully.");
            }
            else
            {
                _logger.LogWarning("Skill ID: {SkillId} not found for deletion.", skillId);
            }
        }
    }
}
