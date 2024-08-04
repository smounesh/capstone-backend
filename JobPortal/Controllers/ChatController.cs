using JobPortal.Models.Dto;
using JobPortal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Controllers
{
    [Route("api/v1/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto chatMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            _logger.LogInformation("Sending message: {Message} from User ID: {SenderID} to User ID: {RecipientID}", chatMessageDto.Message, chatMessageDto.SenderID, chatMessageDto.RecipientID);
            await _chatService.SendMessageAsync(chatMessageDto);
            return NoContent();
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpGet("{senderId}/{recipientId}")]
        public async Task<IActionResult> GetChatHistory(int senderId, int recipientId)
        {
            _logger.LogInformation("Retrieving chat history between User ID: {SenderID} and User ID: {RecipientID}", senderId, recipientId);
            var chatHistory = await _chatService.GetChatHistoryAsync(senderId, recipientId);
            return Ok(chatHistory);
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpPut("{chatMessageId}/seen")]
        public async Task<IActionResult> MarkMessageAsSeen(int chatMessageId)
        {
            _logger.LogInformation("Marking message ID: {ChatMessageID} as seen", chatMessageId);
            await _chatService.MarkMessageAsSeenAsync(chatMessageId);
            return NoContent();
        }
    }
}
