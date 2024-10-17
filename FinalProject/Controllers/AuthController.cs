using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FinalProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHashingService _passwordHashingService;
        private readonly AuthenticationService _authenticationService;

        public AuthController(FinalProjectDbContext context, IMapper mapper, PasswordHashingService passwordHashingService, AuthenticationService authenticationService)
        {
            _context = context;
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
            _authenticationService = authenticationService;

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Hello()
        {
            return Ok(new { Message = "You're Now Accessing the Authorized Page!!" });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto signupDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == signupDto.Username);
            if(existingUser != null)
            {
                return BadRequest("User Already Exists!!");
            }

            var newUser = _mapper.Map<User>(signupDto);
            newUser.Password = _passwordHashingService.HashPassword(newUser.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User Created Successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid Username or Password");
            }

            await _authenticationService.SignInAsync(user.Username);

            return Ok(new { Message = "You have been Logged In Successfully" });
        }

        [HttpPost("{userId}")]
        public IActionResult SetPreferences(int userId, [FromBody] PreferencesDto preferencesDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var preferences = string.Join(",", preferencesDto.SelectedPreferences);

            user.Preferences = preferences;

            _context.SaveChanges();

            return Ok("Preferences saved successfully");
        }

        [HttpGet("{userId}")]
        public IActionResult GetPreferences(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new { user.Preferences });
        }
    }
}
