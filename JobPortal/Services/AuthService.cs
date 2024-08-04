using AutoMapper;
using JobPortal.Enums;
using JobPortal.Exceptions; 
using JobPortal.Helpers;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace JobPortal.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly JWT _jwt;

        public AuthService(IUserRepository userRepository, IMapper mapper, ILogger<AuthService> logger, JWT jwt)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _jwt = jwt;
        }

        public async Task<UserRegistrationResponseDto> SignUpAsync(UserRegistrationDto userRegistrationDto)
        {
            // Check if the user already exists
            var existingUser = await _userRepository.GetByEmailAsync(userRegistrationDto.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(userRegistrationDto.Email);
            }

            // Check if the role is Admin
            if (userRegistrationDto.Role == UserRole.Admin)
            {
                throw new NotAuthorizedException("Admin role is not allowed to sign up.");
            }

            // Map DTO to User model
            var user = _mapper.Map<User>(userRegistrationDto);

            // Hash the password
            CreatePasswordHash(userRegistrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);

            // Save the user to the database
            await _userRepository.AddAsync(user);
            _logger.LogInformation("User signed up successfully: {Email}", user.Email);

            // Map User to UserRegistrationResponseDto
            var responseDto = new UserRegistrationResponseDto
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email
            };

            return responseDto; // Return the response DTO
        }


        public async Task<UserLoginResponseDto> SignInAsync(UserLoginDto userLoginDto)
        {
            _logger.LogInformation("Attempting to sign in user with email: {Email}", userLoginDto.Email);

            // Validate user credentials
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (!VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new InvalidCredentialsException();
            }

            // Generate the authentication token
            var token = _jwt.GenerateToken(user.UserID, user.Name, user.Email, user.Role.ToString());

            // Map User to UserLoginResponseDto
            var responseDto = new UserLoginResponseDto
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };

            _logger.LogInformation("User signed in successfully: {Email}", user.Email);
            return responseDto; // Return the response DTO
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var salt = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
    }
}