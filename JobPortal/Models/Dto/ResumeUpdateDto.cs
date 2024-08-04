using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class ResumeUpdateDto
    {
        public int ResumeID { get; set; } // Unique identifier for the resume
        public IFormFile File { get; set; } // File to be uploaded (optional)
        public ResumeStatus Status { get; set; } // Status of the resume (optional, can be updated if needed)
    }
}
