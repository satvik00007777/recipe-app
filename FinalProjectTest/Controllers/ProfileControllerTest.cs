using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FinalProject.Tests
{
    /// <summary>
    /// Here, all the methods of Profile Controller are tested.
    /// </summary>
    [TestFixture]
    public class ProfileControllerTest
    {
        private Mock<IProfileRepository> _profileRepositoryMock;
        private ProfileController _controller;

        [SetUp]
        public void Setup()
        {
            _profileRepositoryMock = new Mock<IProfileRepository>();
            _controller = new ProfileController(_profileRepositoryMock.Object);
        }

        /// <summary>
        /// This method is used to test whether the user's details are being properly displayed on the Profile page or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProfile_ReturnsOk_WhenUserExists()
        {
            int userId = 1;
            var user = new User
            {
                UserId = userId,
                Name = "John Doe",
                Username = "johndoe",
                Email = "john@example.com"
            };

            _profileRepositoryMock.Setup(repo => repo.GetProfile(userId)).ReturnsAsync(user);

            var result = await _controller.GetProfile(userId);
        }

        /// <summary>
        /// This method checks for the case when user did not match with that in the DB.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProfile_ReturnsBadRequest_WhenUserIdsDoNotMatch()
        {
            var profileDto = new ProfileDto
            {
                UserId = 2,
                Name = "Updated Name",
                Username = "updatedUsername",
                Email = "updated@example.com"
            };

            var userIdFromClaim = 1;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", userIdFromClaim.ToString())
                    }))
                }
            };

            var result = await _controller.UpdateProfile(profileDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// This method checks for the case when user does not exists in the DB.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProfile_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var profileDto = new ProfileDto
            {
                UserId = 1,
                Name = "Updated Name",
                Username = "updatedUsername",
                Email = "updated@example.com"
            };

            var userIdFromClaim = 1;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", userIdFromClaim.ToString())
                    }))
                }
            };

            _profileRepositoryMock.Setup(repo => repo.GetProfile(1)).ReturnsAsync((User)null);

            var result = await _controller.UpdateProfile(profileDto);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        /// <summary>
        /// This method checks for the case when the user's profile is successfully upadted.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProfile_ReturnsOk_WhenUserExists()
        {
            var profileDto = new ProfileDto
            {
                UserId = 1,
                Name = "Updated Name",
                Username = "updatedUsername",
                Email = "updated@example.com"
            };

            var user = new User
            {
                UserId = 1,
                Name = "John Doe",
                Username = "johndoe",
                Email = "john@example.com"
            };

            _profileRepositoryMock.Setup(repo => repo.GetProfile(1)).ReturnsAsync(user);
            _profileRepositoryMock.Setup(repo => repo.UpdateProfile(It.IsAny<User>())).ReturnsAsync(true);

            var userIdFromClaim = 1;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", userIdFromClaim.ToString())
                    }))
                }
            };

            var result = await _controller.UpdateProfile(profileDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
