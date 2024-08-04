using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.Dto
{
    public class ChatMessageDto
    {
        public int ChatMessageID { get; set; } // Unique identifier for the chat message

        [Required(ErrorMessage = "Sender ID is required.")]
        public int SenderID { get; set; } // ID of the user sending the message

        [Required(ErrorMessage = "Recipient ID is required.")]
        public int RecipientID { get; set; } // ID of the user receiving the message

        [Required(ErrorMessage = "Message content is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; } // Content of the message

        public DateTime SentAt { get; set; } // Timestamp of when the message was sent

        public bool IsSeen { get; set; } // Indicates if the message has been seen by the recipient
    }
}
