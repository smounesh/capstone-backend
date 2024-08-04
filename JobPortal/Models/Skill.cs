using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }

        [Required(ErrorMessage = "Skill name is required.")]
        [StringLength(100, ErrorMessage = "Skill name cannot exceed 100 characters.")]
        public string SkillName { get; set; }

        // Foreign key for Profile
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }

        // Additional fields
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Skill level is required.")]
        [StringLength(50, ErrorMessage = "Skill level cannot exceed 50 characters.")]
        public string SkillLevel { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateAcquired { get; set; }

        [StringLength(200, ErrorMessage = "Certification name cannot exceed 200 characters.")]
        public string? Certification { get; set; }
    }
}
