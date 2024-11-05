using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly FinalProjectDbContext _context;

        public ProfileController(FinalProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetProfile")]
        public async Task<ActionResult<ProfileDto>> GetProfile(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if(user == null)
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
            catch (Exception ex)
            {

            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            if(userId != profileDto.UserId)
            {
                return BadRequest(new { Message = "Unable to update this" });
            }

            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                return NotFound(new { Message = "User Not Found" });
            }

            user.Name = profileDto.Name;
            user.Username = profileDto.Username;
            user.Email = profileDto.Email;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Updated Successfully" });
        }

        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst("id")?.Value;

            return Ok(new { UserId = userId });
        }
    }
}
