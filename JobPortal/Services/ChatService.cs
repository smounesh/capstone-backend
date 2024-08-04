using AutoMapper;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task SendMessageAsync(ChatMessageDto chatMessageDto)
        {
            var chatMessage = _mapper.Map<ChatMessage>(chatMessageDto);
            await _chatRepository.SendMessageAsync(chatMessage);
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int senderId, int recipientId)
        {
            var chatMessages = await _chatRepository.GetChatHistoryAsync(senderId, recipientId);
            return _mapper.Map<IEnumerable<ChatMessageDto>>(chatMessages);
        }

        public async Task MarkMessageAsSeenAsync(int chatMessageId)
        {
            await _chatRepository.MarkMessageAsSeenAsync(chatMessageId);
        }
    }
}
