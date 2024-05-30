using DatabaseService.Data;
using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class ChatMembersService : IChatMembersService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserRoleRelationService _userRoleRelationService;

        public ChatMembersService(DatabaseContext databaseContext, IUserRoleRelationService userRoleRelationService)
        {
            _databaseContext = databaseContext;
            _userRoleRelationService = userRoleRelationService;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetChatMembersByChatId(int chatId)
        {
            var chat = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
            var chatInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == chatId);

            var chatMembers = _databaseContext.ChatMembers.Where(x => x.ChatId == chatId).ToList();

            List<UserDto> chatMemberUsersDto = new List<UserDto>();
            foreach (var chatMember in chatMembers)
            {
                var user = await _databaseContext.Users.FirstOrDefaultAsync(x => chatMember.UserId == x.Id);

                if (user == null)
                    continue;

                chatMemberUsersDto.Add(new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName,
                    AvatarURL = user.AvatarURL,
                });
            }
            // just analog
            //List<UserDto> members = _databaseContext.ChatMembers
            //         .Where(cm => cm.ChatId == chatId)
            //         .Select(cm => new UserDto
            //         {
            //             Id = cm.User.Id,
            //             Username = cm.User.Username,
            //             DisplayName = cm.User.DisplayName
            //         })
            //         .ToList();

            if (chat.ChatTypeId == Convert.ToInt32(ChatTypeEnum.group))
            {
                var owner = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == chatInfo.OwnerId);
                chatMemberUsersDto.Add(new UserDto()
                {
                    Id = owner.Id,
                    DisplayName = owner.DisplayName,
                    Username = owner.Username,
                    AvatarURL = owner.AvatarURL,
                });
            }

            return new ServiceResponse<List<UserDto>> { Data = chatMemberUsersDto };
        }

        public async Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDto)
        {
            var alreadyMember = await _databaseContext.ChatMembers
                .AnyAsync(x => x.ChatId == chatMemberDto.ChatId && x.UserId == chatMemberDto.UserId);

            if (alreadyMember)
            {
                return new ServiceResponse<ChatMember>() { Success = false, Message = "User already member" };
            }

            var chatMember = new ChatMember()
            {
                ChatId = chatMemberDto.ChatId,
                UserId = chatMemberDto.UserId,
            };

            _databaseContext.ChatMembers.Add(chatMember);
            await _databaseContext.SaveChangesAsync();

            await _userRoleRelationService.AsignDefaultRole(chatMember.ChatId, chatMember.UserId);

            return new ServiceResponse<ChatMember> { Data = chatMember };
        }

        public async Task<ServiceResponse<ChatMember>> AddMemberByUsername(ChatMemberByUsernameDto chatMemberDto)
        {
            var username = chatMemberDto.Username.Trim().ToLower();

            var userToAdd = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Username.Equals(username));

            if (userToAdd == null)
            {
                return new ServiceResponse<ChatMember>() { Success = false, Message = "This username is not exists" };
            }

            return await AddMember(new ChatMemberDTO() { ChatId = chatMemberDto.ChatId, UserId = userToAdd.Id });
        }

        public async Task<ServiceResponse<bool>> DeleteMember(int chatId, int userId)
        {
            var chatInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == chatId);

            if (chatInfo != null && chatInfo.OwnerId == userId)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = $"Ahah, nice try, but Owner can not be deleted from chat! :D"
                };
            }

            await ClearAllUserRoles(chatId, userId);

            var chatMember = _databaseContext.ChatMembers
                            .FirstOrDefault(x => x.ChatId == chatId &&
                                                 x.UserId == userId);

            if (chatMember == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = $"Chat member with id={userId} is not exists!"
                };
            }

            _databaseContext.ChatMembers.Remove(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        private async Task ClearAllUserRoles(int chatId, int userId)
        {
            var chatRoles = _databaseContext.Roles.Where(x => x.ChatId == chatId).ToList();

            foreach (var chatRole in chatRoles)
            {
                var userRole = await _databaseContext.UserRoleRelations
                    .FirstOrDefaultAsync(x => x.RoleId == chatRole.Id && x.UserId == userId);

                if (userRole != null) _databaseContext.UserRoleRelations.Remove(userRole);
            }

            await _databaseContext.SaveChangesAsync();
        }
    }
}
