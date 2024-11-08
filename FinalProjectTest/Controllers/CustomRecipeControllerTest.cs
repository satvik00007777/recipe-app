using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FinalProjectTest.Controllers
{
    [TestFixture]
    public class CustomRecipeControllerTests
    {
        private CustomRecipeController _controller;
        private FinalProjectDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinalProjectDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new FinalProjectDbContext(options);

            _context.Recipes.AddRange(new List<Recipe>
            {
                new Recipe { RecipeId = 1, Title = "Recipe 1", Ingredients = "Ingredients 1", Instructions = "Instructions 1", Source = "Custom" },
                new Recipe { RecipeId = 2, Title = "Recipe 2", Ingredients = "Ingredients 2", Instructions = "Instructions 2", Source = "Custom" }
            });

            _context.SaveChanges();

            _controller = new CustomRecipeController(_context);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetCustomRecipes_ShouldReturnAllRecipes()
        {
            // Act
            var result = await _controller.GetCustomRecipes() as OkObjectResult;
            var recipes = result?.Value as List<RecipeDto>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, recipes.Count);
            Assert.AreEqual("Recipe 1", recipes[0].Title);
        }
    }
}

