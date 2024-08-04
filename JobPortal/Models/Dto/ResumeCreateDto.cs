using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JobPortal.Models.Dto
{
    public class ResumeCreateDto
    {
        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } // The file to upload
    }
}
