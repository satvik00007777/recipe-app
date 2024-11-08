using AutoMapper;
using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FinalProjectTest.Controllers
{
    [TestFixture]
    public class AuthControllerTest
    {
        //private IMapper _mapper;
        //private PasswordHashingService _passwordHashingService;
        //private TokenService _tokenService;
        //private List<User> _users;
        //private readonly IAccountRepository _accountRepository;
        //private AuthController _authController;

        private Mock<IAccountRepository> _accountRepository;
        private PasswordHashingService _passwordHashingService;
        private Mock<IMapper> _mapperMock;
        private TokenService _tokenService;

        private AuthController _authController;


        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<SignupDto, User>());
            _mapperMock = new Mock<IMapper>();
            _passwordHashingService = new PasswordHashingService();
            _accountRepository = new Mock<IAccountRepository>();

            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                            .AddInMemoryCollection(new Dictionary<string, string> { { "Jwt:key", "hjfgdgfuqUFGhgHfhjewfgwgfWKAGFJWHEFEHWDkfegwfhhfhhufhjhjahfuhffwhfjahfuwehfDFNJfjhj" } })
                            .Build();
            _tokenService = new TokenService(config);

            _authController = new AuthController(_mapperMock.Object, _passwordHashingService, _tokenService,_accountRepository.Object);
        }

        [Test]
        public async Task Signup_ShouldReturnOk_WhenUserIsCreatedSuccessfully()
        {
            var signupDto = new SignupDto { Username = "newuser", Password = "password123", Preferences = "Indian", Email = "abcd@gmail.com", Name = "Hello" };

            //Act
            _accountRepository.Setup(x => x.GetUersByUserName(signupDto.Username)).ReturnsAsync(new User());
            _mapperMock.Setup(m => m.Map<User>(signupDto)).Returns(new User { Username = signupDto.Username, Email = signupDto.Email, Name = signupDto.Name, Password = signupDto.Password, Preferences = signupDto.Preferences });

            var result = await _authController.Signup(signupDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("User Created Successfully.", okResult.Value);

            _accountRepository.Verify(x => x.AddUsers(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task Signup_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            var signupDto = new SignupDto { Username = "existinguser", Password = "password123", Preferences = "Indian", Email = "abcd@gmail.com", Name = "Hello" };

            var existingUser = new User { Username = signupDto.Username };
            _accountRepository.Setup(x => x.GetUersByUserName(signupDto.Username)).ReturnsAsync(existingUser);

            var result = await _authController.Signup(signupDto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User Already Exists!!", badRequestResult.Value);

            _accountRepository.Verify(x => x.AddUsers(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhileCredentialsAreValid()
        {
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "password123"
            };
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            var existingUser = new User { UserId = 1, Username = loginDto.Username, Password = hashedPassword };

            _accountRepository.Setup(x => x.GetUersByUserName(loginDto.Username)).ReturnsAsync(existingUser);

            var result = await _authController.Login(loginDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            var responseValue = okResult.Value as dynamic;
            Assert.AreEqual("You have been successfully logged in now", responseValue.Message);

            //Assert.AreEqual("User Created Successfully.", ((dynamic)okResult.Value).Message);
            Assert.IsNotNull(((dynamic)okResult.Value).Token);
            Assert.IsInstanceOf<string>(((dynamic)okResult.Value).Token);
        }
    }
}
