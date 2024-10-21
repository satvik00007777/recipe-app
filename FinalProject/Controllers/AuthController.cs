using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        private readonly EmailService _emailService;

        public AuthController(FinalProjectDbContext context, IMapper mapper, PasswordHashingService passwordHashingService, AuthenticationService authenticationService, EmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
            _authenticationService = authenticationService;
            _emailService = emailService;
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
            if (existingUser != null)
            {
                return BadRequest("User Already Exists!!");
            }

            var newUser = _mapper.Map<User>(signupDto);
            newUser.Password = _passwordHashingService.HashPassword(newUser.Password);

            _context.Users.Add(newUser);
            try
            {
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {

            }

            return Ok("User Created Successfully. Please check your email to verify your account.");
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

        [HttpPost("SubmitPreferences")]
        [Authorize]
        public async Task<IActionResult> SubmitPreferences([FromBody] string[] categories)
        {
            var userId = GetCurrentUserId();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Save the preferences as a comma-separated string
            user.Preferences = string.Join(",", categories);
            await _context.SaveChangesAsync();

            return Ok("Preferences saved successfully.");
        }

        private int GetCurrentUserId()
        {
            // To get the user ID from the claims
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }

            return int.Parse(claim.Value);
        }
    }
}
