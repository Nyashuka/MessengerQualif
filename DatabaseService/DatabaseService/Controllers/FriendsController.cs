using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService) 
        {
            _friendsService = friendsService;
        }

        [HttpPost("add-friend")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddFriend(FriendRelationDTO friendDTO)
        {
            var response = await _friendsService.AddFriend(friendDTO);

            if (response == null)
                BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get-friends")]
        public async Task<ActionResult<ServiceResponse<bool>>> GetFriends(int userId)
        {
            var response = await _friendsService.GetFriends(userId);

            if (response == null)
                BadRequest(response);

            return Ok(response);
        }

        [HttpPost("remove-friend")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveFriend(FriendRelationDTO friendDTO)
        {
            var response = await _friendsService.RemoveFriend(friendDTO);

            if (response == null)
                BadRequest(response);

            return Ok(response);
        }
    }
}
