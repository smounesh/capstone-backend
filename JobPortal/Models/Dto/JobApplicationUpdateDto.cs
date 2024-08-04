using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class JobApplicationUpdateDto
    {
        public int Id { get; set; } // Unique identifier for the application
        public JobApplicationStatus Status { get; set; } // Status of the application (e.g., Submitted, Reviewed, Interviewed, Rejected, Withdrawn)
    }
}
