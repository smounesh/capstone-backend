using System;
using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class ResumeDto
    {
        public int ResumeID { get; set; } // Unique identifier for the resume
        public int UserID { get; set; } // Foreign key to User
        public string FileName { get; set; } // Name of the uploaded file (hashed)
        public string Url { get; set; } // URL of the resume in Azure Blob Storage
        public ResumeStatus Status { get; set; } // Status of the resume
        public DateTime UploadedAt { get; set; } // Timestamp of when the resume was uploaded
        public string OriginalFileName { get; set; } // The original name of the file
    }
}
