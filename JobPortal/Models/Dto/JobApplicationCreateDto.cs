using System.ComponentModel.DataAnnotations;
using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class JobApplicationCreateDto
    {
        [Required]
        public int JobPostingId { get; set; }
    }
}
