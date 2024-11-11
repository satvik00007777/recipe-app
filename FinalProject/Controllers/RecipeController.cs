using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProject.Controllers
{
    /// <summary>
    /// This controller handles recipe-related operations, including retrieving recipes based on user preferences and retrieving favorite recipes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private IRecipeRepository _recipeRepository;

        /// <summary>
        /// Constructor for RecipeController. Injects HttpClient and IRecipeRepository services for making external API calls and managing user recipe data.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="recipeRepository"></param>
        public RecipeController(HttpClient httpClient, IRecipeRepository recipeRepository) { 
            _httpClient = httpClient;
            _recipeRepository = recipeRepository;
        }

        /// <summary>
        /// Function: GetRecipes
        /// Purpose: Retrieves a list of recipes based on the specified user's preferences by querying an external API.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing a list of recipes or a failure message if the retrieval fails.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Getrecipes")]
        public async Task<IActionResult> GetRecipes(int userId)
        {
            var appId = "bd3f3c6f";
            var appKey = "ffe96fc39d286b9877f112655d467bd9";

            var preferences = await GetUserPreferences(userId);

            var url = $"https://api.edamam.com/search?q={preferences}&app_id={appId}&app_key={appKey}";
            var response = await _httpClient.GetAsync(url);

            if(response.IsSuccessStatusCode)    
            {
                var reponseBody = await response.Content.ReadAsStringAsync();
                var edamamResponse = JsonSerializer.Deserialize<EdamamApiResponse>(reponseBody);

                var recipes = edamamResponse.hits.Select(hit => new Recipe
                {
                    Title = hit.recipe.label,
                    Ingredients = string.Join(", ", hit.recipe.ingredients.Select(i => i.text)),
                    Instructions = hit.recipe.url,
                    ImageUrl = hit.recipe.image,
                    Source = hit.recipe.source
                }).ToList();

                return Ok(recipes);
            }

            return BadRequest("Recipes cannot be retrieved");
        }

        /// <summary>
        /// Function: GetUserPreferences
        /// Purpose: Retrieves user preferences based on the specified user ID from the repository, defaults to "Indian" if preferences are null.
        /// Return Type: Task<string> - An asynchronous task returning the user's preference or a default value if none exists.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<string> GetUserPreferences(int userId)
        {
            var user = await _recipeRepository.FindUser(userId);
            return user?.Preferences ?? "Indian";
        }

        /// <summary>
        /// Function: GetFavourites
        /// Purpose: Retrieves favorite recipes based on a provided query URI by querying an external API.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing a list of favorite recipes or a failure message if the retrieval fails.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet("Getfavourites")]
        public async Task<IActionResult> GetFavourites(string uri)
        {
            var appId = "bd3f3c6f";
            var appKey = "ffe96fc39d286b9877f112655d467bd9";

            var apiUrl = $"https://api.edamam.com/search?q={uri}&app_id={appId}&app_key={appKey}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var reponseBody = await response.Content.ReadAsStringAsync();
                var edamamResponse = JsonSerializer.Deserialize<EdamamApiResponse>(reponseBody);

                var recipe = edamamResponse.hits.Select(hit => new Recipe
                {
                    Title = hit.recipe.label,
                    Ingredients = string.Join(", ", hit.recipe.ingredients.Select(i => i.text)),
                    Instructions = hit.recipe.url,
                    ImageUrl = hit.recipe.image,
                    Source = hit.recipe.source
                });

                return Ok(recipe);
            }

            return BadRequest("Recipes cannot be retrieved");
        }
    }
}
