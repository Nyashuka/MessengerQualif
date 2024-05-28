using ChatsService.Groups.Dto;
using ChatsService.Groups.Models;
using ChatsService.Models;

namespace ChatsService.Groups.Services
{
    public class GroupsInfoService : IGroupsInfoService
    {
        private readonly HttpClient _httpClient;

        public GroupsInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<GroupChatInfoDto>> UpdateGroupInfo(GroupChatInfoDto groupChatInfoDto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"{APIEndpoints.UpdateGroupInfoPATCH}", groupChatInfoDto);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<GroupChatInfoDto>>();

            return data;
        }

        public async Task<ServiceResponse<string>> UpdateGroupPicture(int chatId, string avatarURL)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.UpdateGroupPictureGET}?chatId={chatId}&avatarURL={avatarURL}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            return data;
        }
    }
}
