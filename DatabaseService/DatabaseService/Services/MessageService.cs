using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using System.Text;

namespace DatabaseService.Services
{
    public class MessageService : IMessageService
    {
        private readonly DatabaseContext _databaseContext;

        public MessageService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<List<MessageDto>>> GetAllMessagesByChatId(int chatId)
        {
            var messages = _databaseContext.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new MessageDto()
                {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    Chat = _databaseContext.Chats.FirstOrDefault(c => c.Id == m.ChatId),
                    SenderId = m.SenderId,
                    Sender = _databaseContext.Users.FirstOrDefault(u => u.Id == m.SenderId),
                    RecipientId = m.RecipientId,
                    Recipient = _databaseContext.Users.FirstOrDefault(u => u.Id == m.RecipientId),
                    Data = Encoding.UTF8.GetString(m.Data),
                    MediaUrl = m.MediaUrl,
                    Timestamp = m.Timestamp,
                })
                .ToList();

            return Task.FromResult(new ServiceResponse<List<MessageDto>>()
            {
                Data = messages
            });
        }

        public async Task<ServiceResponse<MessageDto>> SaveMessage(MessageDto messageDto)
        {
            if (!_databaseContext.ChatMembers.Any(cm => cm.ChatId == messageDto.ChatId && cm.UserId == messageDto.SenderId))
                return new ServiceResponse<MessageDto>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Sender is not member of chat!"
                };

            if (!_databaseContext.ChatMembers.Any(cm => cm.ChatId == messageDto.ChatId && cm.UserId == messageDto.RecipientId))
                return new ServiceResponse<MessageDto>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Recepient is not member of chat!"
                };

            Message message = new Message()
            {
                Id = messageDto.Id,
                ChatId = messageDto.ChatId,
                Chat = _databaseContext.Chats.FirstOrDefault(c => c.Id == messageDto.ChatId),
                SenderId = messageDto.SenderId,
                Sender = _databaseContext.Users.FirstOrDefault(u => u.Id == messageDto.SenderId),
                RecipientId = messageDto.RecipientId,
                Recipient = _databaseContext.Users.FirstOrDefault(u => u.Id == messageDto.RecipientId),
                Data = Encoding.UTF8.GetBytes(messageDto.Data),
                MediaUrl = messageDto.MediaUrl,
                Timestamp = messageDto.Timestamp,
            };

            _databaseContext.Messages.Add(message);
            await _databaseContext.SaveChangesAsync();

            MessageDto responseMessage = new MessageDto()
            {
                ChatId = message.ChatId,
                Chat = message.Chat,
                SenderId = message.SenderId,
                Sender = message.Sender,
                RecipientId = message.RecipientId,
                Recipient = message.Recipient,
                Data = Encoding.UTF8.GetString(message.Data),
                MediaUrl = message.MediaUrl,
                Timestamp = message.Timestamp,
            };

            return new ServiceResponse<MessageDto>() { Data = responseMessage };
        }
    }
}
