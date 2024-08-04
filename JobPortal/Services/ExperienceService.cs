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
    public class ExperienceService : IExperienceService
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExperienceService> _logger;

        public ExperienceService(IExperienceRepository experienceRepository, IProfileRepository profileRepository, IMapper mapper, ILogger<ExperienceService> logger)
        {
            _experienceRepository = experienceRepository;
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

        public async Task<IEnumerable<ExperienceDto>> GetExperiencesByUserIdAsync(int userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID: {UserId}", userId);
                throw new ProfileNotFoundException("Profile not found");
            }

            _logger.LogInformation("Retrieving experiences for profile ID: {ProfileId}", profile.ProfileID);
            var experiences = await _experienceRepository.GetByProfileIdAsync(profile.ProfileID);
            return _mapper.Map<IEnumerable<ExperienceDto>>(experiences);
        }


        public async Task<ExperienceDto> CreateExperienceAsync(ClaimsPrincipal user, ExperienceCreateDto experienceDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            // Removed the line that sets ProfileID as ExperienceCreateDto does not have ProfileID
            _logger.LogInformation("Creating experience for profile ID: {ProfileId}", profileId);
            var experience = _mapper.Map<Experience>(experienceDto);
            experience.ProfileID = profileId; // Set ProfileID here instead
            await _experienceRepository.AddAsync(experience);
            return _mapper.Map<ExperienceDto>(experience);
        }

        public async Task<ExperienceDto> UpdateExperienceAsync(ClaimsPrincipal user, ExperienceUpdateDto experienceDto)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            experienceDto.ProfileID = profileId;
            _logger.LogInformation("Updating experience ID: {ExperienceId}", experienceDto.ExperienceID);

            var existingExperience = await _experienceRepository.GetByIdAsync(experienceDto.ExperienceID);
            if (existingExperience == null)
            {
                throw new ExperienceNotFoundException(experienceDto.ExperienceID);
            }

            if (existingExperience.ProfileID != profileId)
            {
                throw new NotAuthorizedException();
            }

            // Update the properties of the existing entity
            _mapper.Map(experienceDto, existingExperience);
            await _experienceRepository.UpdateAsync(existingExperience);
            return _mapper.Map<ExperienceDto>(existingExperience);
        }

        public async Task DeleteExperienceAsync(ClaimsPrincipal user, int experienceId)
        {
            var profileId = await GetProfileIdFromClaimsAsync(user);
            _logger.LogInformation("Deleting experience ID: {ExperienceId} for profile ID: {ProfileId}", experienceId, profileId);

            var existingExperience = await _experienceRepository.GetByIdAsync(experienceId);
            if (existingExperience == null)
            {
                throw new ExperienceNotFoundException(experienceId);
            }

            if (existingExperience.ProfileID != profileId)
            {
                throw new NotAuthorizedException();
            }

            await _experienceRepository.DeleteAsync(experienceId);
        }
    }
}
