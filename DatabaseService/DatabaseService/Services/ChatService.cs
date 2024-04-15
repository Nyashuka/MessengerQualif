using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseService.Services
{
    public class ChatService : IChatService
    {
        private readonly DatabaseContext _databaseContext;
        public ChatService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<Chat>> CreateChat(ChatDTO chatDTO)
        {
            var chatType = _databaseContext.ChatTypes.FirstOrDefault(x => x.Id == chatDTO.ChatTypeId);

            if (chatType == null)
            {
                return new ServiceResponse<Chat>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"Chat type with id={chatDTO.ChatTypeId} is not exists"
                };
            }

            var newChat = new Chat()
            {
                ChatTypeId = chatDTO.ChatTypeId,
                ChatType = chatType
            };

            _databaseContext.Chats.Add(newChat);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<Chat> { Data = newChat };
        }

        public async Task<ServiceResponse<bool>> DeleteChat(int id)
        {
            var chatToUpdate = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == id);

            if (chatToUpdate == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = $"Chat with id={id} is not exists!"
                };
            }

            _databaseContext.Chats.Remove(chatToUpdate);
            await _databaseContext.SaveChangesAsync();
        
            return new ServiceResponse<bool> { Data = true };
        }
    }
}
