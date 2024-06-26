﻿using MessagesService.Chat.Dto;
using MessagesService.Models.Requests;

namespace MessagesService.Services.Interfaces
{
    public interface IChatService
    {
        Task<bool> IsUserChatMember(int chatId, int userId);
        Task<ServiceResponse<ChatDto>> GetChatById(int chatId);
        Task<ServiceResponse<ChatDto>> GetChatByMessageId(int messageId);
    }
}
