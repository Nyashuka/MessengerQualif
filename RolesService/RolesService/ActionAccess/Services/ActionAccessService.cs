﻿using RolesService.ActionAccess.Actions;
using RolesService.Chat.Dto;
using RolesService.Models;

namespace RolesService.ActionAccess.Services
{
    public class ActionAccessService : IActionAccessService
    {
        private readonly HttpClient _httpClient;

        public ActionAccessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> HasAccess<T>(T actionAccess, int chatId, int userId) where T : class, IChatAction
        {
            if (await IsOwner(chatId, userId))
                return true;

            return await actionAccess.HasAccess(_httpClient);
        }

        public async Task<bool> IsOwner(int chatId, int userId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetGroupByIdGET}?chatId={chatId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            if (data.Data.ChatInfo.Owner.Id == userId)
                return true;

            return false;
        }
    }
}