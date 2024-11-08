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
        //private readonly FinalProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHashingService _passwordHashingService;
        private readonly TokenService _tokenService;
        private readonly IAccountRepository _accountRepository;



        public AuthController(IMapper mapper, PasswordHashingService passwordHashingService, TokenService tokenService, IAccountRepository accountRepository)
        {
            //_context = context;
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
            _accountRepository = accountRepository;
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
            var existingUser = await _accountRepository.GetUersByUserName(signupDto.Username);
            if (existingUser.Username != null)
            {
                return BadRequest("User Already Exists!!");
            }

            var newUser = _mapper.Map<User>(signupDto);
            newUser.Password = _passwordHashingService.HashPassword(newUser.Password);
            newUser.Preferences = signupDto.Preferences;

            _accountRepository.AddUsers(newUser);
            

            return Ok("User Created Successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _accountRepository.GetUersByUserName(loginDto.Username);
            if (user.Username == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid Username or Password");
            }

            var token = _tokenService.GenerateToken(user.UserId);

            return Ok(new { Token = token, Message = "You have been successfully logged in now" });
        }

        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst("id")?.Value;

            return Ok(new { UserId = userId });
        }
    }
}
