using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using FinalProject.Services;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FinalProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHashingService _passwordHashingService;

        public AuthController(FinalProjectDbContext context, IMapper mapper, PasswordHashingService passwordHashingService)
        {
            _context = context;
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
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

            return Ok(new { Message = "You have been Logged In Successfully" });
        }
    }
}
