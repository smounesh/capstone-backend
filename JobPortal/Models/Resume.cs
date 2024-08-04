using JobPortal.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Resume
    {
        [Key]
        public int ResumeID { get; set; } // Unique identifier for the resume

        public int UserID { get; set; } // Foreign key to User

        [Required]
        public string FileName { get; set; } // Name of the uploaded file

        [Required]
        public string Url { get; set; } // URL of the resume in Azure Blob Storage

        public string OriginalFileName { get; set; }

        public ResumeStatus Status { get; set; } = ResumeStatus.Active; // Status of the resume

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow; // Timestamp of when the resume was uploaded

        // Navigation property
        public virtual User User { get; set; } // Navigation to User
    }
}
