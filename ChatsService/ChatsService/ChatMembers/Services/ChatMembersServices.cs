using ChatsService.ChatMembers.Dto;
using ChatsService.ChatMembers.Models;
using ChatsService.Models;

namespace ChatsService.ChatMembers.Services
{
    public class ChatMembersServices : IChatMembersService
    {
        private readonly HttpClient _httpClient;

        public ChatMembersServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDto chatMemberDto)
        {
            var databaseResponse = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.AddMemberToChatPOST}", chatMemberDto);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatMember>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> DeleteMember(int chatId, int userId)
        {
            var databaseResponse = await _httpClient
                .DeleteAsync($"{APIEndpoints.DeleteMemberDELETE}?chatId={chatId}&userId={userId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetChatMembersByChatId(int chatId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatMembersGET}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<UserDto>>>();

            return data;
        }
    }
}
