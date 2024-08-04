using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class JobApplicationDto
    {
        public int Id { get; set; } // Unique identifier for the application
        public int UserId { get; set; } // Identifier for the user applying
        public int JobPostingId { get; set; }
        public string username { get; set; } // Identifier for the applicant's resume
        public JobApplicationStatus Status { get; set; } // Status of the application (e.g., Submitted, Reviewed, Interviewed, Rejected)
        public DateTime ApplicationDate { get; set; } // Date of application submission
    }
}
