using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Education
    {
        [Key]
        public int EducationID { get; set; }

        [Required(ErrorMessage = "Profile ID is required.")]
        public int ProfileID { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        [StringLength(255, ErrorMessage = "Degree cannot exceed 255 characters.")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Institution is required.")]
        [StringLength(255, ErrorMessage = "Institution cannot exceed 255 characters.")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Field of Study is required.")]
        [StringLength(255, ErrorMessage = "Field of Study cannot exceed 255 characters.")]
        public string FieldOfStudy { get; set; }

        [Range(1900, int.MaxValue, ErrorMessage = "Start year must be a valid year.")]
        public int StartYear { get; set; }

        [Range(1900, int.MaxValue, ErrorMessage = "End year must be a valid year.")]
        public int EndYear { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Profile Profile { get; set; }
    }
}
