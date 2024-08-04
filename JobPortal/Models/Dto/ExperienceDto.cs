namespace JobPortal.Models.Dto
{
    public class ExperienceDto
    {
        public int ExperienceID { get; set; } // Unique identifier
        public int ProfileID { get; set; } // Foreign key referencing Profile
        public string JobTitle { get; set; } // Job title
        public string Company { get; set; } // Company name
        public DateTime StartDate { get; set; } // Start date of employment
        public DateTime? EndDate { get; set; } // End date of employment (nullable)
        public string Description { get; set; } // Job description
    }
}
