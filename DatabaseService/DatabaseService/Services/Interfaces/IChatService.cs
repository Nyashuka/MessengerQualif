﻿using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IChatService
    {

        Task<ServiceResponse<ChatDto>> GetPersonalChatIfExists(List<UserDto> users);
        Task<ServiceResponse<List<ChatDto>>> GetAllPersonalChats(int userId);
        Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDTO);
        Task<ServiceResponse<bool>> DeleteChat(int id);
        Task<ServiceResponse<ChatDto>> GetChatById(int chatId);
        Task<ServiceResponse<ChatDto>> GetChatByMessageId(int messageId);
        Task<ServiceResponse<ChatDto>> GetChatByRoleId(int roleId);
    }
}
