using JobPortal.Enums;
using JobPortal.Models;
using JobPortal.Models.Dto;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileByUserIdAsync(int userId);
        Task<(bool IsConflict, string Message, Profile Profile)> CreateProfileAsync(int userId, ProfileCreateDto profileCreateDto);
        Task<Profile> UpdateProfileAsync(int userId, ProfileUpdateDto profileUpdateDto);
        Task DeleteProfileAsync(int userId, DeletedBy deletedBy);
        Task<bool> UserHasProfileAsync(int userId);
    }
}
