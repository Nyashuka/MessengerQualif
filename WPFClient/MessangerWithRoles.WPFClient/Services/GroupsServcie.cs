using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ServiceResponse<ChatDto>> CreateGroup(CreateGroupChatDto chatDto)
        {
            chatDto.ChatTypeId = Convert.ToInt32(ChatTypeEnum.group);
            chatDto.ChatInfo.Owner = _authService.User;
            chatDto.ChatInfo.AvatarUrl = "";

            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.CreateGroupPOST}?accessToken={_authService.AccessToken}", chatDto);
            
            if(!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<ChatDto> { Success = false, Message = response.ReasonPhrase };
            }

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            if(data == null)
                data = new ServiceResponse<ChatDto> { Success = false };

            return data;
        }

        public async Task<ServiceResponse<List<ChatDto>>> GetGroups()
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllGroupsGET}?accessToken={_authService.AccessToken}");
            
            if(!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<List<ChatDto>>()
                {
                    Success = false,
                    Message = response.ReasonPhrase
                };
            }

            var chatListResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ChatDto>>>();

            return chatListResponse;
        }

        public async Task<GroupViewModel> GroupModelToViewModel(ChatDto group)
        {
            var chatsService = ServiceLocator.Instance.GetService<ChatsService>();

            var messages = await chatsService.GetChatMessages(group.Id);

            return new GroupViewModel(group.Id ,group.ChatInfo.Name, group.ChatInfo.Description, new ObservableCollection<User>(group.Members), messages);
        }
    }
}
