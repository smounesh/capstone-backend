using AutoMapper;
using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SkillService> _logger;

        public SkillService(ISkillRepository skillRepository, IProfileRepository profileRepository, IMapper mapper, ILogger<SkillService> logger)
        {
            _skillRepository = skillRepository;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task<int> GetProfileIdFromClaimsAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier); // Fetches the 'sub' claim
            if (string.IsNullOrEmpty(userId))
            {
                throw new System.InvalidOperationException("User ID claim is missing.");
            }
            var profile = await _profileRepository.GetByUserIdAsync(int.Parse(userId));
            return profile?.ProfileID ?? 0;
        }

        public async Task<IEnumerable<SkillDto>> GetSkillsByProfileIdAsync(int profileId)
        {
            _logger.LogInformation("Retrieving skills for profile ID: {ProfileId}", profileId);
            var skills = await _skillRepository.GetByProfileIdAsync(profileId);
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        public async Task<SkillDto> CreateSkillAsync(ClaimsPrincipal user, SkillCreateDto skillDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            _logger.LogInformation("Creating skill for profile ID: {ProfileId}", profileId);
            var skill = _mapper.Map<Skill>(skillDto);
            skill.ProfileID = profileId; // Set ProfileID here
            await _skillRepository.AddAsync(skill);
            return _mapper.Map<SkillDto>(skill);
        }

        public async Task<SkillDto> UpdateSkillAsync(ClaimsPrincipal user, SkillUpdateDto skillDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            skillDto.ProfileID = profileId;
            _logger.LogInformation("Updating skill ID: {SkillId}", skillDto.SkillID);

            var existingSkill = await _skillRepository.GetByIdAsync(skillDto.SkillID);
            if (existingSkill == null)
            {
                throw new SkillNotFoundException(skillDto.SkillID);
            }

            if (existingSkill.ProfileID != profileId)
            {
                throw new NotAuthorizedException();
            }

            // Update the properties of the existing entity
            _mapper.Map(skillDto, existingSkill);
            await _skillRepository.UpdateAsync(existingSkill);
            return _mapper.Map<SkillDto>(existingSkill);
        }

        public async Task DeleteSkillAsync(ClaimsPrincipal user, int skillId)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            _logger.LogInformation("Deleting skill ID: {SkillId} for profile ID: {ProfileId}", skillId, profileId);

            var existingSkill = await _skillRepository.GetByIdAsync(skillId);
            if (existingSkill == null)
            {
                throw new SkillNotFoundException(skillId);
            }

            if (existingSkill.ProfileID != profileId)
            {
                throw new NotAuthorizedException();
            }

            await _skillRepository.DeleteAsync(skillId);
        }
        public async Task<Skill> GetSkillByIdAsync(int skillId)
        {
            return await _skillRepository.GetByIdAsync(skillId);
        }
    }
}
