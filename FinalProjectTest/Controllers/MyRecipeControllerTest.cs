using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinalProject.Tests
{
    /// <summary>
    /// Here all the methods of MyRecipeController are tested.
    /// </summary>
    [TestFixture]
    public class MyRecipeControllerTest
    {
        private Mock<IMyRecipeRepository> _mockRecipeRepository;
        private MyRecipeController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRecipeRepository = new Mock<IMyRecipeRepository>();
            _controller = new MyRecipeController(_mockRecipeRepository.Object);
        }

        /// <summary>
        /// This method checks whether the recipes are properly fetched or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetCustomRecipesByUser_ReturnsRecipes_WhenRecipesExist()
        {
            int userId = 1;
            var recipes = new List<Recipe>
            {
                new Recipe { RecipeId = 1, Title = "Recipe1", Ingredients = "Ingredient1, Ingredient2", Instructions = "Step 1, Step 2", UserId = userId },
                new Recipe { RecipeId = 2, Title = "Recipe2", Ingredients = "Ingredient3, Ingredient4", Instructions = "Step 1, Step 2", UserId = userId }
            };
            _mockRecipeRepository.Setup(repo => repo.GetCustomRecipesByUser(userId)).ReturnsAsync(recipes);

            var result = await _controller.GetCustomRecipesByUser(userId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var returnedRecipes = result.Value as List<RecipeDto>;
            Assert.AreEqual(2, returnedRecipes.Count);
            Assert.AreEqual("Recipe1", returnedRecipes[0].Title);
        }

        /// <summary>
        /// This method checks for the case when the Recipe does not exists in the Database.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetCustomRecipesByUser_ReturnsEmptyList_WhenNoRecipesExist()
        {
            int userId = 2;
            _mockRecipeRepository.Setup(repo => repo.GetCustomRecipesByUser(userId)).ReturnsAsync(new List<Recipe>());

            var result = await _controller.GetCustomRecipesByUser(userId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var returnedRecipes = result.Value as List<RecipeDto>;
            Assert.IsNotNull(returnedRecipes);
            Assert.AreEqual(0, returnedRecipes.Count);
        }

        /// <summary>
        /// This method returns Ok result with correct properties.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetCustomRecipesByUser_ReturnsOkResult_WithCorrectProperties()
        {
            int userId = 3;
            var recipes = new List<Recipe>
            {
                new Recipe { RecipeId = 3, Title = "Special Recipe", Ingredients = "IngredientX", Instructions = "Step X", UserId = userId, ImageUrl = "http://image.url", Source = "CustomSource" }
            };
            _mockRecipeRepository.Setup(repo => repo.GetCustomRecipesByUser(userId)).ReturnsAsync(recipes);

            var result = await _controller.GetCustomRecipesByUser(userId) as OkObjectResult;

            Assert.IsNotNull(result);
            var returnedRecipes = result.Value as List<RecipeDto>;
            Assert.AreEqual(1, returnedRecipes.Count);
            Assert.AreEqual("Special Recipe", returnedRecipes[0].Title);
            Assert.AreEqual("IngredientX", returnedRecipes[0].Ingredients);
            Assert.AreEqual("Step X", returnedRecipes[0].Instructions);
            Assert.AreEqual("http://image.url", returnedRecipes[0].ImageUrl);
            Assert.AreEqual("CustomSource", returnedRecipes[0].Source);
        }

        [Test]
        public async Task GetCustomRecipesByUser_ReturnsDefaultSource_WhenSourceIsNull()
        {
            int userId = 4;
            var recipes = new List<Recipe>
            {
                new Recipe { RecipeId = 4, Title = "Recipe without Source", Ingredients = "Ingredients List", Instructions = "Some Instructions", UserId = userId, ImageUrl = null, Source = null }
            };
            _mockRecipeRepository.Setup(repo => repo.GetCustomRecipesByUser(userId)).ReturnsAsync(recipes);

            var result = await _controller.GetCustomRecipesByUser(userId) as OkObjectResult;

            Assert.IsNotNull(result);
            var returnedRecipes = result.Value as List<RecipeDto>;
            Assert.AreEqual(1, returnedRecipes.Count);
            Assert.AreEqual("Custom", returnedRecipes[0].Source);
        }
    }
}
