using FinalProjectMVC.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FinalProjectMVC.Controllers
{
    public class RecipeController : Controller
    {
        private readonly HttpClient _httpClient;

        public RecipeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetLoggedInUserId();

            var recipes = await GetRecipesFromApi(userId);

            return View(recipes);
        }

        private int GetLoggedInUserId()
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            if(User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if(userIdClaim != null)
                {
                    return int.Parse(userIdClaim.Value);
                }
            }

            return 0;
        }

        private async Task<List<RecipeDto>> GetRecipesFromApi(int userId)
        {
            var apiUrl = $"https://localhost:7255/api/recipe/getrecipes?userId={userId}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<RecipeDto>>(responseBody);
            }

            return new List<RecipeDto>();
        }

    }
}
