using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            //if(User.Identity.IsAuthenticated)
            //{
            //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //    if(userIdClaim != null)
            //    {
            //        return int.Parse(userIdClaim.Value);
            //    }
            //}

            //var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            //if(userIdClaim != null)
            //{
            //    var userId = int.Parse(userIdClaim.Value);

            //    return userId;
            //}

            var user = _apiClientService.GetAuthenticatedClient();
            var response = await user.GetAsync("auth/userinfo");

            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(responseData);

                if(userInfo != null && userInfo.TryGetValue("UserId", out string userId))
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
                return JsonSerializer.Deserialize<List<RecipeDto>>(responseBody);
            }

            return new List<RecipeDto>();
        }

    }
}
