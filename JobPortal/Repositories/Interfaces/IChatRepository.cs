using JobPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task SendMessageAsync(ChatMessage chatMessage); // Send a chat message
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int senderId, int recipientId); // Get chat history between two users
        Task MarkMessageAsSeenAsync(int chatMessageId); // Mark a message as seen
    }
}
