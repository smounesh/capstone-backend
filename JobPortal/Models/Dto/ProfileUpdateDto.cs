using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.Dto
{
    public class ProfileUpdateDto
    {
        // Base64 encoded image string
        public IFormFile ProfilePictureBase64 { get; set; }

        [Required(ErrorMessage = "Headline is required.")]
        [StringLength(255, ErrorMessage = "Headline cannot exceed 255 characters.")]
        public string? Headline { get; set; }

        [StringLength(65535, ErrorMessage = "Summary cannot exceed 65535 characters.")]
        public string? Summary { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string? Location { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL format.")]
        [StringLength(255, ErrorMessage = "LinkedIn URL cannot exceed 255 characters.")]
        public string? LinkedinUrl { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL format.")]
        [StringLength(255, ErrorMessage = "GitHub URL cannot exceed 255 characters.")]
        public string? GitHubUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Years of experience must be a non-negative integer.")]
        public int YearsOfExperience { get; set; }
    }
}
