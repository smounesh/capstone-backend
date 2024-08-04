using JobPortal.Models;
using JobPortal.Models.Dto;

namespace JobPortal.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRegistrationResponseDto> SignUpAsync(UserRegistrationDto userRegistrationDto);
        Task<UserLoginResponseDto> SignInAsync(UserLoginDto userLoginDto);
    }
}
