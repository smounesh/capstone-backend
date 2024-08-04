using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class ChatMessage
    {
        [Key]
        public int ChatMessageID { get; set; } // Unique identifier for the chat message

        [Required(ErrorMessage = "Sender ID is required.")]
        public int SenderID { get; set; } // ID of the user sending the message

        [Required(ErrorMessage = "Recipient ID is required.")]
        public int RecipientID { get; set; } // ID of the user receiving the message

        [Required(ErrorMessage = "Message content is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; } // Content of the message

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow; // Timestamp of when the message was sent

        public bool IsSeen { get; set; } = false; // Indicates if the message has been seen by the recipient

        // Navigation properties
        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; } // Navigation to the sender

        [ForeignKey("RecipientID")]
        public virtual User Recipient { get; set; } // Navigation to the recipient
    }
}
