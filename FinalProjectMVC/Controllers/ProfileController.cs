using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FinalProjectMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApiClientService _apiClientService;

        public ProfileController(HttpClient httpClient, ApiClientService apiClientService)
        {
            _httpClient = httpClient;
            _apiClientService = apiClientService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetLoggedInUserId();

            var response = await _httpClient.GetAsync($"https://localhost:7255/api/Profile/GetProfile?userId={userId}");

            if(response.IsSuccessStatusCode)
            {
                var profile = await response.Content.ReadFromJsonAsync<ProfileDto>();
                return View(profile);
            } else
            {
                ViewBag.Error = "Error";
            }

            return View();
        }

        public async Task<IActionResult> Edit()
        {
            int userId = await GetLoggedInUserId();
            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.GetAsync($"https://localhost:7255/api/Profile/GetProfile?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var profile = await response.Content.ReadFromJsonAsync<ProfileDto>();
                return View(profile);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            var client = _apiClientService.GetAuthenticatedClient();
            var jsonContent = JsonSerializer.Serialize(profileDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7255/api/Profile", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Failed to update profile.";
            return View("Edit", profileDto);
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
                } catch(Exception ex)
                {
                    throw ex;
                }
            }

            return 0;
        }
    }
}
