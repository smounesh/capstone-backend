using AutoMapper;
using JobPortal.Enums;
using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper, ILogger<ProfileService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProfileDto> GetProfileByUserIdAsync(int userId)
        {
            _logger.LogInformation("Getting profile for user ID {UserId}", userId);
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID {UserId}", userId);
                throw new ProfileNotFoundException("Profile not found");
            }

            return _mapper.Map<ProfileDto>(profile);
        }

        public async Task<(bool IsConflict, string Message, JobPortal.Models.Profile Profile)> CreateProfileAsync(int userId, ProfileCreateDto profileCreateDto)
        {
            _logger.LogInformation("Creating profile for user ID {UserId}", userId);
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            if (existingProfile != null)
            {
                _logger.LogWarning("User profile already exists for user ID {UserId}", userId);
                return (true, "User profile already exists.", existingProfile);
            }

            var profile = _mapper.Map<JobPortal.Models.Profile>(profileCreateDto);
            profile.UserID = userId;

            // Get name and email from security claims
            var httpContext = _httpContextAccessor.HttpContext;
            foreach (var claim in httpContext.User.Claims)
            {
                _logger.LogInformation("Claim Type: {ClaimType}, Claim Value: {ClaimValue}", claim.Type, claim.Value);
            }

            if (httpContext != null && httpContext.User != null)
            {
                var name = httpContext.User.FindFirst("name")?.Value;
                var email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                _logger.LogInformation($"{name}: {email}");

                profile.Name = name;
                profile.Email = email;
            }
            else
            {
                _logger.LogWarning("HttpContext or User is null. Unable to retrieve name and email from claims.");
            }

            if (profileCreateDto.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileCreateDto.ProfilePicture.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    profile.ProfilePictureBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            // Log profile fields
            _logger.LogInformation("Profile created with the following details: " +
                                   "UserID: {UserID}, Name: {Name}, Email: {Email}, Location: {Location}, " +
                                   "Headline: {Headline}, Summary: {Summary}, PhoneNumber: {PhoneNumber}, " +
                                   "LinkedinUrl: {LinkedinUrl}, GitHubUrl: {GitHubUrl}, YearsOfExperience: {YearsOfExperience}",
                                   profile.UserID, profile.Name, profile.Email, profile.Location,
                                   profile.Headline, profile.Summary, profile.PhoneNumber,
                                   profile.LinkedinUrl, profile.GitHubUrl, profile.YearsOfExperience);

            await _profileRepository.AddAsync(profile);
            _logger.LogInformation("Profile created for user ID {UserId}", userId);
            return (false, string.Empty, profile);
        }

        public async Task<JobPortal.Models.Profile> UpdateProfileAsync(int userId, ProfileUpdateDto profileUpdateDto)
        {
            _logger.LogInformation("Updating profile for user ID {UserId}", userId);
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            if (existingProfile == null)
            {
                _logger.LogWarning("Profile not found for user ID {UserId}", userId);
                throw new ProfileNotFoundException("Profile not found");
            }

            _mapper.Map(profileUpdateDto, existingProfile);

            if (profileUpdateDto.ProfilePictureBase64 != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileUpdateDto.ProfilePictureBase64.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    existingProfile.ProfilePictureBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            await _profileRepository.UpdateAsync(existingProfile);
            _logger.LogInformation("Profile updated for user ID {UserId}", userId);
            return existingProfile;
        }

        public async Task DeleteProfileAsync(int userId, DeletedBy deletedBy)
        {
            _logger.LogInformation("User ID {UserId} has opted for account deletion. The account will be deleted after 30 days unless they log in before that.", userId);
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            if (existingProfile == null)
            {
                _logger.LogWarning("Profile not found for user ID {UserId}", userId);
                throw new ProfileNotFoundException("Profile not found");
            }

            await _profileRepository.DeleteAsync(existingProfile.ProfileID, deletedBy);
            _logger.LogInformation("Profile for user ID {UserId} marked for deletion. If the user logs in before the deletion date, the account deletion will be canceled.", userId);
        }

        public async Task<bool> UserHasProfileAsync(int userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            return profile != null;
        }
    }
}
