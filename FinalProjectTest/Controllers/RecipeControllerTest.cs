using FinalProject.Controllers;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace FinalProjectTest.Controllers
{
    /// <summary>
    /// Here, all the methods or Recipe Controller are tested.
    /// </summary>
    public class RecipeControllerTest
    {
        private RecipeController _controller;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void SetUp()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _controller = new RecipeController(httpClient, _recipeRepositoryMock.Object);
        }

        /// <summary>
        /// This methods checks whether the API call for GetRecipes is successful or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetRecipes_ReturnsOkResultWithRecipes_WhenApiCallIsSuccessful()
        {
            int userId = 1;
            var preferences = "Italian";
            _recipeRepositoryMock.Setup(repo => repo.FindUser(userId))
                .ReturnsAsync(new User { UserId = userId, Preferences = preferences });

            var expectedRecipes = new List<Recipe>
            {
                new Recipe { Title = "Pasta", Ingredients = "Tomato, Basil", Instructions = "some_url", ImageUrl = "image_url", Source = "source" }
            };

            var edamamApiResponse = new
            {
                hits = expectedRecipes.Select(r => new
                {
                    recipe = new
                    {
                        label = r.Title,
                        ingredients = r.Ingredients.Split(", ").Select(i => new { text = i }),
                        url = r.Instructions,
                        image = r.ImageUrl,
                        source = r.Source
                    }
                })
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(edamamApiResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _controller.GetRecipes(userId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var recipes = okResult.Value as List<Recipe>;
            Assert.IsNotNull(recipes);
            Assert.AreEqual(expectedRecipes.Count, recipes.Count);
            Assert.AreEqual(expectedRecipes[0].Title, recipes[0].Title);
            Assert.AreEqual(expectedRecipes[0].Ingredients, recipes[0].Ingredients);
            Assert.AreEqual(expectedRecipes[0].Instructions, recipes[0].Instructions);
            Assert.AreEqual(expectedRecipes[0].ImageUrl, recipes[0].ImageUrl);
            Assert.AreEqual(expectedRecipes[0].Source, recipes[0].Source);
        }

        /// <summary>
        /// This method tests for the case when the API call for the GetRecipes is failed.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetRecipes_ReturnsBadRequest_WhenApiCallFails()
        {
            int userId = 1;
            _recipeRepositoryMock.Setup(repo => repo.FindUser(userId))
                .ReturnsAsync(new User { UserId = userId, Preferences = "Italian" });

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _controller.GetRecipes(userId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Recipes cannot be retrieved", badRequestResult.Value);
        }

        /// <summary>
        /// This methods checks whether the API call for GetFavourites is successful or not.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetFavourites_ReturnsOkResultWithRecipes_WhenApiCallIsSuccessful()
        {
            string uri = "pasta";
            var expectedRecipes = new List<Recipe>
            {
                new Recipe { Title = "Pasta", Ingredients = "Tomato, Basil", Instructions = "some_url", ImageUrl = "image_url", Source = "source" }
            };

            var edamamApiResponse = new
            {
                hits = expectedRecipes.Select(r => new
                {
                    recipe = new
                    {
                        label = r.Title,
                        ingredients = r.Ingredients.Split(", ").Select(i => new { text = i }),
                        url = r.Instructions,
                        image = r.ImageUrl,
                        source = r.Source
                    }
                })
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(edamamApiResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _controller.GetFavourites(uri);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var recipes = okResult.Value as IEnumerable<Recipe>;
            Assert.IsNotNull(recipes);
            Assert.AreEqual(expectedRecipes.Count, recipes.Count());
            Assert.AreEqual(expectedRecipes.First().Title, recipes.First().Title);
            Assert.AreEqual(expectedRecipes.First().Ingredients, recipes.First().Ingredients);
            Assert.AreEqual(expectedRecipes.First().Instructions, recipes.First().Instructions);
            Assert.AreEqual(expectedRecipes.First().ImageUrl, recipes.First().ImageUrl);
            Assert.AreEqual(expectedRecipes.First().Source, recipes.First().Source);
        }

        /// <summary>
        /// This method tests for the case when the API call for the GetFavourites is failed.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetFavourites_ReturnsBadRequest_WhenApiCallFails()
        {
            string uri = "pasta";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _controller.GetFavourites(uri);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Recipes cannot be retrieved", badRequestResult.Value);
        }
    }
}
