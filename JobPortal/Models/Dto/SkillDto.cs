namespace JobPortal.Models.Dto
{
    public class SkillDto
    {
        public int SkillID { get; set; } // Unique identifier
        public int ProfileID { get; set; } // Foreign key referencing Profile
        public string SkillName { get; set; } // Skill name
        public string Description { get; set; } // Description of the skill
        public string SkillLevel { get; set; } // Skill level
        public DateTime? DateAcquired { get; set; } // Date when the skill was acquired
        public string? Certification { get; set; } // Certification related to the skill
    }
}
