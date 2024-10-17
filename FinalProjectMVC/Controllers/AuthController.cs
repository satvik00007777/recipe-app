using FinalProjectMVC.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;

namespace FinalProjectMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7255/api/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/Login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Recipe");
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(signupDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/signup", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Preference");
            }

            ViewBag.ErrorMessage = "User already exists!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Preferences()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult SavePreferences(List<string> selectedPreferences)
        {
            var userId = 1;

            var preferences = string.Join(",", selectedPreferences);

            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Preferences = preferences;
                _context.SaveChanges();
            }

            return RedirectToAction("RecipePage", "Recipes");
        }
    }
}
