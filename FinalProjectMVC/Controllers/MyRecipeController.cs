using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProjectMVC.Controllers
{
    public class MyRecipeController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public MyRecipeController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetLoggedInUserId();

            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.GetAsync($"https://localhost:7255/api/MyRecipe/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var recipes = await response.Content.ReadFromJsonAsync<List<CustomRecipeDto>>();
                return View(recipes);
            }
            else
            {
                ViewBag.Error = "Error fetching recipes.";
                return View();
            }
        }

        private async Task<int> GetLoggedInUserId()
        {
            var user = _apiClientService.GetAuthenticatedClient();
            var response = await user.GetAsync("auth/userinfo");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(responseData);

                try
                {
                    if (userInfo != null && userInfo.TryGetValue("userId", out string userId))
                    {
                        var a = int.Parse(userId);
                        return a;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return 0;
        }
    }
}
