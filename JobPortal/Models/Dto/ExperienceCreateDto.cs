namespace JobPortal.Models.Dto
{
    public class ExperienceCreateDto
    {
        public string JobTitle { get; set; } // Required job title
        public string Company { get; set; } // Required company name
        public DateTime StartDate { get; set; } // Required start date
        public DateTime? EndDate { get; set; } // Nullable for current jobs
        public string Description { get; set; } // Optional description
    }
}
