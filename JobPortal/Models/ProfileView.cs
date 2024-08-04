using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class ProfileView
    {
        [Key]
        public int ProfileViewID { get; set; } // Unique identifier for the profile view record

        [Required]
        public int ViewerID { get; set; } // ID of the user who viewed the profile

        [Required]
        public int ProfileID { get; set; } // ID of the profile that was viewed

        [Required]
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow; // Timestamp of when the profile was viewed

        // Navigation properties
        [ForeignKey("ViewerID")]
        public virtual User? Viewer { get; set; } // Navigation property to the Viewer

        [ForeignKey("ProfileID")]
        public virtual Profile? Profile { get; set; } // Navigation property to the Profile
    }
}
