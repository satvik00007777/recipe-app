using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RecipeController(HttpClient httpClient) { 
            _httpClient = httpClient;
        }

        public async Task<IActionResult> GetRecipes(string preferences)
        {
            var appId = "";
            var appKey = "";

            var url = $"";
            var response = await _httpClient.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var edamamResponse = JsonSerializer.Deserialize<EdamamApiResponse>(responseBody);

                var recipes = edamamResponse.hits.Select(hit => new Recipe
                {
                    Title = hit.recipe.Title,
                    Ingredients = string.Join(", ", hit.recipe.Ingredients),
                    Instructions = hit.recipe.ImageUrl,
                    ImageUrl = hit.recipe.ImageUrl,
                    Source = hit.recipe.Source
                }).ToList();

                return Ok(recipes);
            }

            return BadRequest("Failed to retrieve recipes.");
        }
    }
}
