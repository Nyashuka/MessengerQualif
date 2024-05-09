using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class ChatTypeService : IChatTypeService
    {
        private readonly DatabaseContext _databaseContext;

        public ChatTypeService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<ChatType>> CreateChatType(ChatTypeDTO dto)
        {
            var chatType = new ChatType() { Name = dto.Name };

            _databaseContext.Add(chatType);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<ChatType>() { Data = chatType };
        }

        public async Task<ServiceResponse<ChatType>> UpdateChatType(ChatType dto)
        {
            var chatType = await _databaseContext.ChatTypes.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (chatType == null)
            {
                return new ServiceResponse<ChatType>() { 
                    Data = null, 
                    Success = false, 
                    Message = $"Chat type with id={dto.Id} does not exists!" 
                };
            }
        
            chatType.Name = dto.Name;

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<ChatType> { Data = chatType };
        }

        public async Task<ServiceResponse<bool>> DeleteChatType(int dto)
        {
            var chatType = await _databaseContext.ChatTypes.FirstOrDefaultAsync(x => x.Id == dto);

            if (chatType == null)
            {
                return new ServiceResponse<bool>() { Data = false, Success = false, Message = "" };
            }

            _databaseContext.ChatTypes.Remove(chatType);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }

    }
}
