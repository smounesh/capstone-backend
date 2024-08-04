using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobPortal.Enums;

namespace JobPortal.Models
{
    public class Profile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Profile picture is required.")]
        public string ProfilePictureBase64 { get; set; }

        [StringLength(255, ErrorMessage = "Headline cannot exceed 255 characters.")]
        public string Headline { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "Summary cannot exceed 65535 characters.")]
        public string Summary { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string Location { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string PhoneNumber { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL format.")]
        [StringLength(255, ErrorMessage = "LinkedIn URL cannot exceed 255 characters.")]
        public string LinkedinUrl { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL format.")]
        [StringLength(255, ErrorMessage = "GitHub URL cannot exceed 255 characters.")]
        public string GitHubUrl { get; set; }

        // Skills as a collection for better management
        public virtual ICollection<Skill> Skills { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Years of experience must be a non-negative integer.")]
        public int YearsOfExperience { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Soft delete fields
        public DateTime? DeletedAt { get; set; }
        public DeletedBy? DeletedBy { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
    }
}