using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly JobPortalContext _context;

        public ChatRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task SendMessageAsync(ChatMessage chatMessage)
        {
            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int senderId, int recipientId)
        {
            return await _context.ChatMessages
                .Include(cm => cm.Sender) // Include sender details
                .Include(cm => cm.Recipient) // Include recipient details
                .Where(cm => (cm.SenderID == senderId && cm.RecipientID == recipientId) ||
                             (cm.SenderID == recipientId && cm.RecipientID == senderId))
                .OrderBy(cm => cm.SentAt) // Order messages by sent time
                .ToListAsync();
        }

        public async Task MarkMessageAsSeenAsync(int chatMessageId)
        {
            var message = await _context.ChatMessages.FindAsync(chatMessageId);
            if (message != null)
            {
                message.IsSeen = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
