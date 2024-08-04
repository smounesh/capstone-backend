using JobPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.Dto
{
    public class JobPostingUpdateDto
    {
        public int JobID { get; set; }

        [StringLength(255, ErrorMessage = "Job title cannot exceed 255 characters.")]
        public string Title { get; set; }

        public string Description { get; set; } // Optional

        public List<string> Requirements { get; set; } // Optional

        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
        public string Company { get; set; }

        [StringLength(255, ErrorMessage = "Company logo URL cannot exceed 255 characters.")]
        public string CompanyLogo { get; set; } // Optional

        public JobType? JobType { get; set; } // Optional

        [StringLength(100, ErrorMessage = "Salary range cannot exceed 100 characters.")]
        public string SalaryRange { get; set; } // Optional

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string Location { get; set; } // Optional

        [StringLength(100, ErrorMessage = "Industry cannot exceed 100 characters.")]
        public string Industry { get; set; } // Optional

        [StringLength(100, ErrorMessage = "Job function cannot exceed 100 characters.")]
        public string JobFunction { get; set; } // Optional

        public SeniorityLevel? SeniorityLevel { get; set; } // Optional

        public DateTime? ExpirationDate { get; set; } // Optional

        [Required(ErrorMessage = "Job posting status is required.")]
        public JobStatus JobStatus { get; set; } // Required for updates
    }
}