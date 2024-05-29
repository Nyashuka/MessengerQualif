using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerWithRoles.WPFClient.Services
{
    public class GroupsServcie : IService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public GroupsServcie()
        {
            _httpClient = new HttpClient();
            _authService = ServiceLocator.Instance.GetService<AuthService>();
        }

        public async Task<ServiceResponse<Chat>> CreateGroup(CreateGroupChatDto chatDto)
        {
            chatDto.ChatTypeId = Convert.ToInt32(ChatTypeEnum.group);
            chatDto.ChatInfo.Owner = _authService.User;
            chatDto.ChatInfo.AvatarUrl = "";

            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.CreateGroupPOST}?accessToken={_authService.AccessToken}", chatDto);

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<Chat> { Success = false, Message = response.ReasonPhrase };
            }

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<Chat>>();

            if (data.Data == null)
                data = new ServiceResponse<Chat> { Success = false, Message = "Can't parse data from create group response" };

            return data;
        }

        public async Task<ServiceResponse<List<Chat>>> GetGroups()
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllGroupsGET}?accessToken={_authService.AccessToken}");

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<List<Chat>>()
                {
                    Success = false,
                    Message = response.ReasonPhrase
                };
            }

            var chatListResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Chat>>>();

            return chatListResponse;
        }

        public async Task<GroupViewModel> GroupModelToViewModel(Chat group)
        {
            var chatsService = ServiceLocator.Instance.GetService<PersonalChatsService>();
            var rolesService = ServiceLocator.Instance.GetService<RolesService>();


            var messages = await chatsService.GetChatMessages(group.Id);
            var roles = await rolesService.GetChatRoles(group.Id);

            return new GroupViewModel(group.Id,
                    group.ChatInfo.Name,
                    group.ChatInfo.Description,
                    group.ChatInfo.AvatarUrl,
                    new ObservableCollection<User>(group.Members),
                    messages,
                    new ObservableCollection<RoleWithPermissions>(roles.Data));
        }

        public async Task<ServiceResponse<bool>> DeleteMember(int chatId, int userId)
        {
            var response = await _httpClient
                .DeleteAsync($"{APIEndpoints.DeleteChatMemberDELETE}?accessToken={_authService.AccessToken}&chatId={chatId}&userId={userId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<ChatMember>> AddMemberByUsername(int chatId, string username)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.AddChatMemberByUsernamePOST}?accessToken={_authService.AccessToken}",
                new AddChatMemberByUsernameDto() { ChatId = chatId, Username = username });

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<ChatMember>>();

            return data;
        }

        public async Task<ServiceResponse<List<ChatPermission>>> GetAllPermissions()
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllPermissionsGET}?accessToken={_authService.AccessToken}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ChatPermission>>>();

            return data;
        }

        public async Task<ServiceResponse<GroupChatInfoDto>> UpdateGroupInfo(GroupChatInfoDto groupChatInfoDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.UpdateGroupChatInfoPOST}", groupChatInfoDto);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<GroupChatInfoDto>>();

            return data;
        }

        public async Task<ServiceResponse<string>> UpdatePicture(int chatId, string filePath)
        {
            var file = File.OpenRead(filePath);
            var fileContent = new StreamContent(file);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileContent, "file", Path.GetFileName(filePath));

                var authService = ServiceLocator.Instance.GetService<AuthService>();

                var response = await client
                    .PostAsync($"{APIEndpoints.UpdateGroupChatPicture}?accessToken={authService.AccessToken}&chatId={chatId}", formData);

                var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

                if (!responseData.Success)
                    return responseData;

                return new ServiceResponse<string>()
                {
                    Data = $"{APIEndpoints.ChatsServer}/{responseData.Data}?timestamp={DateTime.Now.Ticks}"
                };

            }
        }

        public async Task<ServiceResponse<GroupChatInfoDto>> UpdateInfo(GroupChatInfoDto groupChatInfoDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.UpdateGroupChatInfoPOST}?accessToken={_authService.AccessToken}", groupChatInfoDto);
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<GroupChatInfoDto>>();

            return data;
        }
    }
}
