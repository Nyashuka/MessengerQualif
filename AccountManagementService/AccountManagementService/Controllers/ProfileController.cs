using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IProfileService _profileService;

        public ProfileController(IAuthService authService, IProfileService profileService)
        {
            _authService = authService;
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<User>>> UpdateInfo([FromQuery] string accessToken, UserDto userDto)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            var response = await _profileService.UpdateProfileInfo(userDto);

            return Ok(response);
        }

        [HttpPost("picture")]
        public async Task<ActionResult<ServiceResponse<string>>> UploadImage([FromQuery] string accessToken, IFormFile file)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ServiceResponse<bool>()
                {
                    Success = true,
                    Message = "No file uploaded."
                });
            }

            var filePath = await SavePicture(authenticatedUser.Data.UserId, file);

            var response = await _profileService.UpdateProfilePicture(authenticatedUser.Data.UserId, filePath);

            return Ok(response);
        }

        private async Task<string> SavePicture(int userId, IFormFile file)
        {
            var directoryPath = Path.Combine("uploads", "profile", userId.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, "avatar.jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return filePath;
        }
    }
}
