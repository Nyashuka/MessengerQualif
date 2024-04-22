using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsManagementController : ControllerBase
    {
        private readonly IFriendsManagementService _friendsManagementService;
        private readonly IAuthService _authService;

        public FriendsManagementController(IFriendsManagementService friendsManagementController, IAuthService authService)
        {
            _friendsManagementService = friendsManagementController;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetFriends(string accessToken)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authenticatedUser.Success == false)
            {
                var badResponse = new ServiceResponse<List<User>>
                {
                    Success = false,
                    ErrorMessage = authenticatedUser.ErrorMessage
                };
                return BadRequest(badResponse);
            }

            var response = await _friendsManagementService.GetFriends(authenticatedUser.Data.Data.UserId);

            if (response.Data == null || !response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("add-friend")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddFriend(string accessToken, int friendUserId)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authenticatedUser.Success == false)
            {
                var badResponse = new ServiceResponse<bool>()
                {
                    Success = false,
                    ErrorMessage = authenticatedUser.ErrorMessage
                };

                return BadRequest(badResponse);
            }

            var response = await _friendsManagementService.AddFriend(authenticatedUser.Data.Data.UserId, friendUserId);

            if(!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("remove-friend")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveFriend(string accessToken, int friendUserId)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authenticatedUser.Success == false)
            {
                var badResponse = new ServiceResponse<bool>
                {
                    Success = false,
                    ErrorMessage = authenticatedUser.ErrorMessage
                };
                return BadRequest(badResponse);
            }

            var response = await _friendsManagementService.RemoveFriend(authenticatedUser.Data.Data.UserId, friendUserId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
