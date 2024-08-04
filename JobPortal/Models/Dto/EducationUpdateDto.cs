namespace JobPortal.Models.Dto
{
    public class EducationUpdateDto
    {
        public int EducationID { get; set; } // Required for updates
        public string Degree { get; set; } // Required degree information
        public string Institution { get; set; } // Required institution name
        public string FieldOfStudy { get; set; } // Required field of study
        public int StartYear { get; set; } // Required start year
        public int EndYear { get; set; } // Required end year
    }
}

