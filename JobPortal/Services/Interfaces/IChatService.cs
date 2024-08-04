using JobPortal.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services.Interfaces
{
    public interface IChatService
    {
        Task SendMessageAsync(ChatMessageDto chatMessageDto); // Send a chat message
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int senderId, int recipientId); // Get chat history between two users
        Task MarkMessageAsSeenAsync(int chatMessageId); // Mark a message as seen
    }
}
