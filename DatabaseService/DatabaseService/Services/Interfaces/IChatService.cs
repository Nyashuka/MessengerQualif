﻿using DatabaseService.DTOs;
using DatabaseService.Models;

namespace DatabaseService.Services.Interfaces
{
    public interface IChatService
    {
        Task<ServiceResponse<Chat>> CreateChat(ChatDTO chatDTO);
        Task<ServiceResponse<bool>> DeleteChat(int id);
    }
}