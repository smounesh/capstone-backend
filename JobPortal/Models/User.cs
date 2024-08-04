using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JobPortal.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace JobPortal.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Password salt is required.")]
        [StringLength(255, ErrorMessage = "Password salt cannot exceed 255 characters.")]
        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public UserRole Role { get; set; }

        // Navigation property
        public virtual Profile Profile { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}