using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Experience
    {
        [Key]
        public int ExperienceID { get; set; }

        [Required(ErrorMessage = "Profile ID is required.")]
        public int ProfileID { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(255, ErrorMessage = "Job title cannot exceed 255 characters.")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; } // Nullable for currently employed

        [StringLength(int.MaxValue, ErrorMessage = "Description cannot exceed 65535 characters.")]
        public string Description { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Profile Profile { get; set; }
    }
}
