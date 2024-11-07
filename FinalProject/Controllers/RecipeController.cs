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
        private readonly FinalProjectDbContext _context;

        public RecipeController(HttpClient httpClient, FinalProjectDbContext context) { 
            _httpClient = httpClient;
            _context = context;
        }

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
                    Source = hit.recipe.source,
                    Uri = hit.recipe.uri
                }).ToList();

                return Ok(recipes);
            }

            return BadRequest("Recipes cannot be retrieved");
        }

        private async Task<string> GetUserPreferences(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Preferences ?? "Indian";
        }
    }
}
