using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobPortal.Enums;

namespace JobPortal.Models
{
    public class JobPosting
    {
        [Key]
        public int JobID { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(255, ErrorMessage = "Job title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Job description is required.")]
        public string Description { get; set; } // Consider using a larger type if needed

        [Required(ErrorMessage = "Requirements are required.")]
        public List<string> Requirements { get; set; } // Changed to List<string> for multiple requirements

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Company logo is required.")]
        [StringLength(255, ErrorMessage = "Company logo URL cannot exceed 255 characters.")]
        public string CompanyLogo { get; set; } // Added property for company logo

        [Required(ErrorMessage = "Job type is required.")]
        public JobType JobType { get; set; }

        [Required(ErrorMessage = "Salary range is required.")]
        [StringLength(100, ErrorMessage = "Salary range cannot exceed 100 characters.")]
        public string SalaryRange { get; set; }

        [Required(ErrorMessage = "Job location is required.")]
        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Industry is required.")]
        [StringLength(100, ErrorMessage = "Industry cannot exceed 100 characters.")]
        public string Industry { get; set; }

        [Required(ErrorMessage = "Job function is required.")]
        [StringLength(100, ErrorMessage = "Job function cannot exceed 100 characters.")]
        public string JobFunction { get; set; }

        [Required(ErrorMessage = "Seniority level is required.")]
        public SeniorityLevel SeniorityLevel { get; set; }

        [Required(ErrorMessage = "Posted date is required.")]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow; // Default to current date

        public DateTime? ExpirationDate { get; set; } // Optional

        [Required(ErrorMessage = "Job posting status is required.")]
        public JobStatus JobStatus { get; set; }

        public int ViewCount { get; set; } = 0; // Default to 0

        public int ApplicationCount { get; set; } = 0; // Default to 0

        [Required(ErrorMessage = "Posted by user ID is required.")]
        public int PostedBy { get; set; } // Foreign key referencing Users table

        // Navigation property
        public virtual User User { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}