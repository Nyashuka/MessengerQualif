﻿using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ServiceResponse<MessageDto>> SaveMessage(MessageDto messageDto);
        Task<ServiceResponse<List<MessageDto>>> GetAllMessagesByChatId(int chatId);
    }
}
