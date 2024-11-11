using FinalProject.Controllers;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinalProject.Tests
{
    /// <summary>
    /// Here all the custom recipe methods are tested.
    /// </summary>
    [TestFixture]
    public class CustomRecipeControllerTests
    {
        private CustomRecipeController _controller;
        private Mock<ICustomRecipeRepository> _customRecipeRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _customRecipeRepositoryMock = new Mock<ICustomRecipeRepository>();
            _controller = new CustomRecipeController(_customRecipeRepositoryMock.Object);
        }

        /// <summary>
        /// This method is used to test whether the Recipes are being properly fetched or not
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetCustomRecipes_ReturnsOkResultWithRecipes()
        {
            var recipes = new List<Recipe>
            {
                new Recipe { RecipeId = 1, Title = "Pasta", Ingredients = "Tomato, Basil", Instructions = "Boil pasta", Source = "Custom" },
                new Recipe { RecipeId = 2, Title = "Pizza", Ingredients = "Cheese, Tomato", Instructions = "Bake pizza", Source = "Custom" }
            };

            _customRecipeRepositoryMock.Setup(repo => repo.GetCustomRecipes())
                .ReturnsAsync(recipes);

            var result = await _controller.GetCustomRecipes();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var returnedRecipes = okResult.Value as List<RecipeDto>;
            Assert.AreEqual(recipes.Count, returnedRecipes.Count);
            Assert.AreEqual(recipes[0].Title, returnedRecipes[0].Title);
            Assert.AreEqual(recipes[1].Title, returnedRecipes[1].Title);
        }

        /// <summary>
        /// This method is used to check whether the model checks are valid or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Create_ReturnsOkResult_WhenModelStateIsValid()
        {
            var recipeDto = new RecipeDto
            {
                Title = "Salad",
                Ingredients = "Lettuce, Tomato",
                Instructions = "Mix all ingredients",
                ImageUrl = "image_url",
                Source = "Custom"
            };

            var recipe = new Recipe
            {
                Title = recipeDto.Title,
                Ingredients = recipeDto.Ingredients,
                Instructions = recipeDto.Instructions,
                ImageUrl = recipeDto.ImageUrl,
                Source = recipeDto.Source,
                UserId = null
            };

            _customRecipeRepositoryMock.Setup(repo => repo.CreateRecipe(It.IsAny<Recipe>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(recipeDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            _customRecipeRepositoryMock.Verify(r => r.CreateRecipe(It.Is<Recipe>(rec =>
                rec.Title == recipe.Title &&
                rec.Ingredients == recipe.Ingredients &&
                rec.Instructions == recipe.Instructions &&
                rec.ImageUrl == recipe.ImageUrl &&
                rec.Source == recipe.Source
            )), Times.Once);
        }

        /// <summary>
        /// This method tests the case when the model states are invalid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Title is required");
            var recipeDto = new RecipeDto();

            var result = await _controller.Create(recipeDto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsInstanceOf<SerializableError>(badRequestResult.Value);
        }

        /// <summary>
        /// This method checks the update process being completed when the recipe exists in the Database.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Update_ReturnsOkResult_WhenRecipeExists()
        {
            int recipeId = 1;
            var recipeDto = new RecipeDto
            {
                Title = "Updated Salad",
                Ingredients = "Lettuce, Cucumber",
                Instructions = "Mix and serve",
                ImageUrl = "updated_image_url",
                Source = "Custom"
            };

            var existingRecipe = new Recipe
            {
                RecipeId = recipeId,
                Title = "Salad",
                Ingredients = "Lettuce, Tomato",
                Instructions = "Mix ingredients",
                Source = "Custom"
            };

            _customRecipeRepositoryMock.Setup(repo => repo.GetCustomRecipe(recipeId))
                .ReturnsAsync(existingRecipe);
            _customRecipeRepositoryMock.Setup(repo => repo.UpdateRecipe(It.IsAny<Recipe>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(recipeId, recipeDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            _customRecipeRepositoryMock.Verify(r => r.UpdateRecipe(It.Is<Recipe>(rec =>
                rec.Title == recipeDto.Title &&
                rec.Ingredients == recipeDto.Ingredients &&
                rec.Instructions == recipeDto.Instructions &&
                rec.ImageUrl == recipeDto.ImageUrl &&
                rec.Source == recipeDto.Source
            )), Times.Once);
        }

        /// <summary>
        /// This method checks for the case when the recipe is not found in the Database.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Update_ReturnsNotFound_WhenRecipeDoesNotExist()
        {
            int recipeId = 1;
            var recipeDto = new RecipeDto
            {
                Title = "Nonexistent Recipe",
                Ingredients = "None",
                Instructions = "None",
                ImageUrl = "none",
                Source = "Custom"
            };

            _customRecipeRepositoryMock.Setup(repo => repo.GetCustomRecipe(recipeId))
                .ReturnsAsync((Recipe)null);

            var result = await _controller.Update(recipeId, recipeDto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        /// <summary>
        /// This method checks whether the recipe is properly deleted or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Delete_ReturnsOkResult_WhenRecipeIsDeleted()
        {
            int recipeId = 1;
            _customRecipeRepositoryMock.Setup(repo => repo.DeleteRecipe(recipeId))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(recipeId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Recipe has been successfully Deleted!!", okResult.Value);
            _customRecipeRepositoryMock.Verify(r => r.DeleteRecipe(recipeId), Times.Once);
        }
    }
}
