using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProjectMVC.Controllers
{
    public class RecipeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiClientService _apiClientService;

        public RecipeController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ApiClientService apiClientService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _apiClientService = apiClientService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetLoggedInUserId();

            var recipes = await GetRecipesFromApi(userId);

            return View(recipes);
        }

        private async Task<int> GetLoggedInUserId()
        {
            var user = _apiClientService.GetAuthenticatedClient();
            var response = await user.GetAsync("auth/userinfo");

            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(responseData);

                if(userInfo != null && userInfo.TryGetValue("userId", out string userId))
                {
                    return int.Parse(userId);
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
                var a = JsonSerializer.Deserialize<List<RecipeDto>>(responseBody);
                return a;
            }

            return new List<RecipeDto>();
        }

    }
}
