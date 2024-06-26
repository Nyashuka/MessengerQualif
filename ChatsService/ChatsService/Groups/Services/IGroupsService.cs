﻿using ChatsService.Models;
using ChatsService.Groups.Dto;
using ChatsService.Groups.Models;

namespace ChatsService.Groups.Services
{
    public interface IGroupsService
    {
        Task<ServiceResponse<Chat>> CreateGroup(ChatDto chatDto);
        Task<ServiceResponse<List<Chat>>> GetAllGroupsByUserId(int userId);
    }
}