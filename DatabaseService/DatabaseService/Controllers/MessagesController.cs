using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet] 
        public async Task<ActionResult<ServiceResponse<List<MessageDto>>>> GetAllMessagesByChatId(int chatId)
        {
            var response = await _messageService.GetAllMessagesByChatId(chatId);

            return Ok(response);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<ServiceResponse<MessageDto>>> GetMessageById(int messageId)
        {
            var response = await _messageService.GetMessageById(messageId);

            return Ok(response);
        }

        [HttpPost("save")]
        public async Task<ActionResult<ServiceResponse<MessageDto>>> SaveMessage(MessageDto messageDto)
        {
            var response = await _messageService.SaveMessage(messageDto);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteMessage(int messageId)
        {
            var response = await _messageService.DeleteMessage(messageId);

            return Ok(response);
        }
    }
}
