//using AutoMapper;
//using FinalProject.Controllers;
//using FinalProject.DTOs;
//using FinalProject.Models;
//using FinalProject.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Moq;

//namespace FinalProjectTest.Controllers
//{
//    internal class AuthControllerTest
//    {
//        private Mock<FinalProjectDbContext> _contextMock;
//        private Mock<IMapper> _mapperMock;
//        private Mock<PasswordHashingService> _passwordHashingServiceMock;
//        private Mock<AuthenticationService> _authenticationServiceMock;
//        private Mock<TokenService> _tokenServiceMock;

//        private AuthController _authController;
//        private PasswordHashingService passwordHashingService;

//        [SetUp]
//        public void Setup()
//        {
//            _contextMock = new Mock<FinalProjectDbContext>();
//            _mapperMock = new Mock<IMapper>();
//            _passwordHashingServiceMock = new Mock<PasswordHashingService>();
//            passwordHashingService = new PasswordHashingService();

//            var configMock = new Mock<IConfiguration>();
//            configMock.Setup(config => config["Jwt:key"]).Returns("hjfgdgfuqUFGhgHfhjewfgwgfWKAGFJWHEFEHWDkfegwfhhfhhufhjhjahfuhffwhfjahfuwehfDFNJfjhj");

//            var tokenService = new TokenService(configMock.Object);

//            _authController = new AuthController(
//                _contextMock.Object,
//                _mapperMock.Object,
//                _passwordHashingServiceMock.Object,
//                tokenService
//            );
//        }

//        [Test]
//        public async Task Signup_ShouldReturnOk_WheneverIsCreatedSuccessfully()
//        {
//            var a = _contextMock.Object.Users.ToList();
//            var signupDto = new SignupDto { Username = "user", Password = "password123", Preferences = "Indian" };

//            //_contextMock.Setup(ctx => ctx.Users.FirstOrDefaultAsync(u => u.Username == signupDto.Username))
//            //    .Returns((User)null);

            

//            //var newUser = new User{
//            //    Username = "newUser",
//            //    Password = "password123"
//            //}; 
//            //_contextMock.Setup(c => c.Users.FirstOrDefaultAsync(u => u.Username == signupDto.Username)).ReturnsAsync((User)null);

//            _contextMock.Setup(ctx => ctx.Users.Add(It.IsAny<User>()));
//            _contextMock.Setup(ctx => ctx.SaveChangesAsync(default)).ReturnsAsync(1);

//            //var newUser = new User { Username = signupDto.Username, Password = signupDto.Password };
//            //_mapperMock.Setup(m => m.Map<User>(signupDto)).Returns(newUser);
//            //passwordHashingService.HashPassword(signupDto.Password);

//            // Act
//            var result = await _authController.Signup(signupDto);

//            // Assert
//            Assert.IsInstanceOf<OkObjectResult>(result);
//            Assert.AreEqual("User Created Successfully.", ((OkObjectResult)result).Value);
//        }
//    }
//}

