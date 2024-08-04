using JobPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.Dto
{
    public class JobPostingCreateDto
    {
        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(255, ErrorMessage = "Job title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Job description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Requirements are required.")]
        public List<string> Requirements { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Company logo is required.")]
        [StringLength(255, ErrorMessage = "Company logo URL cannot exceed 255 characters.")]
        public string CompanyLogo { get; set; }

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

        public DateTime? ExpirationDate { get; set; } // Optional
    }
}
