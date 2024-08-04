namespace JobPortal.Models.Dto
{
    public class EducationDto
    {
        public int EducationID { get; set; } // Unique identifier
        public int ProfileID { get; set; } // Foreign key referencing Profile
        public string Degree { get; set; } // Degree obtained
        public string Institution { get; set; } // Name of the institution
        public string FieldOfStudy { get; set; } // Field of study
        public int StartYear { get; set; } // Start year of education
        public int EndYear { get; set; } // End year of education
    }
}

