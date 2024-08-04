using JobPortal.Enums;

namespace JobPortal.Models
{
    public class JobApplication
    {
        public int Id { get; set; } // Unique identifier for the application
        public int UserId { get; set; } // Identifier for the user applying
        public string username { get; set; } // Identifier for the applicant's resume
        public int JobPostingId { get; set; } // Identifier for the job posting being applied to
        public JobApplicationStatus Status { get; set; } // Status of the application (e.g., Submitted, Reviewed, Interviewed, Rejected)
        public DateTime ApplicationDate { get; set; } // Date of application submission

        // Navigation properties
        public virtual User User { get; set; } // Navigation to User
        public virtual JobPosting JobPosting { get; set; } // Navigation to JobPosting
    }
}
