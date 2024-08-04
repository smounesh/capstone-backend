using JobPortal.Exceptions;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/auth")] // Hard code version to v1 in the route
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            try
            {
                var responseDto = await _authService.SignUpAsync(userRegistrationDto);
                return Ok(responseDto); // Return the response DTO
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogWarning(ex.Message);
                return Conflict(ex.Message); // Return 409 Conflict for existing user
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during signup for email: {Email}", userRegistrationDto.Email);
                return BadRequest(ex.Message); // Return generic error message
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            try
            {
                var responseDto = await _authService.SignInAsync(userLoginDto);
                return Ok(responseDto); // Return the response DTO
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message); // Return 404 Not Found for user not found
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex.Message);
                return Unauthorized(ex.Message); // Return 401 Unauthorized for invalid credentials
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during signin for email: {Email}", userLoginDto.Email);
                return BadRequest(ex.Message); // Return generic error message
            }
        }
    }
}
