using FinalProject.DTOs;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    /// <summary>
    /// This controller manages user profile operations, including retrieving, updating, and getting user information.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfileRepository _profileRepository;

        /// <summary>
        /// Constructor for ProfileController. Injects the IProfileRepository service for accessing profile data.
        /// </summary>
        /// <param name="profileRepository"></param>
        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        /// <summary>
        /// Function: GetProfile
        /// Purpose: Retrieves the profile details of a user based on the provided user ID.
        /// Return Type: Task<ActionResult<ProfileDto>> - An asynchronous action result containing the user's profile or NotFound if the user does not exist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetProfile")]
        public async Task<ActionResult<ProfileDto>> GetProfile(int userId)
        {
            var user = await _profileRepository.GetProfile(userId);

            if (user == null)
            {
                return NotFound(new { Message = "User Not Found" });
            }

            var profile = new ProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
            };

            return Ok(profile);

        }

        /// <summary>
        /// Function: UpdateProfile
        /// Purpose: Updates the profile details of a user if the user ID matches the authenticated user's ID.
        /// Return Type: Task<IActionResult> - An asynchronous action result indicating the success or failure of the update operation.
        /// </summary>
        /// <param name="profileDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            if(userId != profileDto.UserId)
            {
                return BadRequest(new { Message = "Unable to update this" });
            }

            var user = await _profileRepository.GetProfile(userId);

            if (user == null)
            {
                return NotFound(new { Message = "User Not Found" });
            }

            user.Name = profileDto.Name;
            user.Username = profileDto.Username;
            user.Email = profileDto.Email;

            var updateSuccessful = await _profileRepository.UpdateProfile(user);

            return Ok(new { Message = "Updated Successfully" });
        }

        /// <summary>
        /// Function: GetUserInfo
        /// Purpose: Retrieves the authenticated user's ID from the JWT token and returns it in the response.
        /// Return Type: IActionResult - A synchronous action result containing the user's ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst("id")?.Value;

            return Ok(new { UserId = userId });
        }
    }
}
