using AutoMapper;
using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace JobPortal.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _educationRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EducationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EducationService(
            IEducationRepository educationRepository,
            IProfileRepository profileRepository,
            IMapper mapper,
            ILogger<EducationService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _educationRepository = educationRepository;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IEnumerable<EducationDto>> GetEducationsByProfileIdAsync(int profileId)
        {
            _logger.LogInformation("Retrieving educations for profile ID: {ProfileId}", profileId);
            var educations = await _educationRepository.GetByProfileIdAsync(profileId);
            return _mapper.Map<IEnumerable<EducationDto>>(educations);
        }

        public async Task<EducationDto> CreateEducationAsync(int profileId, EducationCreateDto educationDto)
        {
            _logger.LogInformation("Creating education for profile ID: {ProfileId}", profileId);
            var education = _mapper.Map<Education>(educationDto);
            education.ProfileID = profileId;
            await _educationRepository.AddAsync(education);
            return _mapper.Map<EducationDto>(education);
        }

        public async Task<EducationDto> UpdateEducationAsync(int educationId, EducationUpdateDto educationDto)
        {
            // Retrieve userId from HttpContext
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
            {
                _logger.LogWarning("HttpContext or User is null. Unable to retrieve user ID.");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found.");
                throw new UnauthorizedAccessException("User ID claim not found.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Invalid user ID claim value.");
                throw new UnauthorizedAccessException("Invalid user ID claim value.");
            }

            // Retrieve profileId using userId
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID: {UserId}", userId);
                throw new KeyNotFoundException("Profile not found.");
            }

            int profileId = profile.ProfileID;

            // Retrieve existing education and cross-check profileId
            var existingEducation = await _educationRepository.GetByIdAndProfileIdAsync(educationId, profileId);
            if (existingEducation == null)
            {
                _logger.LogWarning("Education ID: {EducationId} not found for profile ID: {ProfileId}", educationId, profileId);
                throw new EducationNotFoundException();
            }

            _logger.LogInformation("Updating education ID: {EducationId} for profile ID: {ProfileId}", educationId, profileId);

            // Map the updated values from the DTO to the entity
            _mapper.Map(educationDto, existingEducation);

            // Save the updated education entity
            await _educationRepository.UpdateAsync(existingEducation);

            return _mapper.Map<EducationDto>(existingEducation);
        }

        public async Task DeleteEducationAsync(int profileId, int educationId)
        {
            var existingEducation = await _educationRepository.GetByIdAndProfileIdAsync(educationId, profileId);
            if (existingEducation == null)
            {
                _logger.LogWarning("Education ID: {EducationId} not found for profile ID: {ProfileId}", educationId, profileId);
                throw new EducationNotFoundException();
            }

            _logger.LogInformation("Deleting education ID: {EducationId} for profile ID: {ProfileId}", educationId, profileId);
            await _educationRepository.DeleteAsync(educationId);
        }
    }
}
