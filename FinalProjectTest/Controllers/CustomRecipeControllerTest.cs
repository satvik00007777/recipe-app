using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using FinalProject.Controllers;


namespace FinalProjectTest.Controllers
{
    internal class CustomRecipeControllerTest
    {
        private Mock<FinalProjectDbContext> _contextMock;
        private CustomRecipeController _customRecipeController;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<FinalProjectDbContext>();

            _customRecipeController = new CustomRecipeController(
                _contextMock.Object
            );
        }

        [Test]
        public async Task GetRecipes()
        {
            var a = _contextMock.Object.Users;
        }
    }
}
