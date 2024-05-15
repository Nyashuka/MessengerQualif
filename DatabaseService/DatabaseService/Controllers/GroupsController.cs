using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpPost] 
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreateGroup(ChatDto chat)
        {
            var response = await _groupsService.CreateGroup(chat);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ChatDto>>>> GetAllGroupsByUserId(int userId)
        {
            var response = await _groupsService.GetAllGroups(userId);

            return Ok(response);
        }

        [HttpGet("group-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetGroupById(int chatId)
        {
            var response = await _groupsService.GetChatById(chatId);

            return Ok(response);
        }
    }
}
