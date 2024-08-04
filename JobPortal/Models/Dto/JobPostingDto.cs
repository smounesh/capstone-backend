using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class JobPostingDto
    {
        public int JobID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> Requirements { get; set; }

        public string Company { get; set; }

        public string CompanyLogo { get; set; }

        public JobType JobType { get; set; }

        public string SalaryRange { get; set; }

        public string Location { get; set; }

        public string Industry { get; set; }

        public string JobFunction { get; set; }

        public SeniorityLevel SeniorityLevel { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public JobStatus JobStatus { get; set; }

        public int ViewCount { get; set; }

        public int ApplicationCount { get; set; }

        public int PostedBy { get; set; }
    }
}
