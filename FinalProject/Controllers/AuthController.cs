using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    /// <summary>
    /// This class is basically handling the logics for Authenticaion -> Login and Signup
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PasswordHashingService _passwordHashingService;
        private readonly TokenService _tokenService;
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// In this constructor, services such as IMapper and custom services like PasswordHashingService, TokenService, and IAccountRepository are being injected for use in the AuthController.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="passwordHashingService"></param>
        /// <param name="tokenService"></param>
        /// <param name="accountRepository"></param>
        public AuthController(IMapper mapper, PasswordHashingService passwordHashingService, TokenService tokenService, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Funtion: Signup
        /// Purpose: This endpoint handles the user signup process. It checks if the username already exists, hashes the password, maps the SignupDto to a User object, and adds the new user to the repository.
        /// Return Type: Task<IActionResult> - An asynchronous action result indicating success or failure of the signup process.
        /// </summary>
        /// <param name="signupDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Function: Login
        /// Purpose: This endpoint handles user login. It checks if the provided username exists, verifies the password using BCrypt, and generates a token if the login is successful.
        /// Return Type: Task<IActionResult> - An asynchronous action result indicating success or failure of the login process.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Function: GetUserInfo
        /// Purpose: This endpoint retrieves the user information from the JWT token, specifically the user ID, and returns it in the resposne.
        /// Return Type: IActionResult  A synchronous action result indicating the success of retrieving user info.
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
