using FinalProjectMVC.DTOs;
using Microsoft.AspNetCore.Mvc;
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
            if(!ModelState.IsValid)
            {
                return View(signupDto);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(signupDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/Signup", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Preferences", "Auth");
            }

            ViewBag.ErrorMessage = "User already exists!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCategories(string[] categories)
        {
            // To check whether min. seleted categories are 3 or not.
            if (categories.Length < 3)
            {
                ViewBag.ErrorMessage = "Please select at least 3 categories.";
                return View("SelectCategories");
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(categories), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Auth/SubmitPreferences", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Recipe"); // Redirect to the recipe page or wherever needed
            }

            ViewBag.ErrorMessage = "Failed to submit categories.";
            return View("SelectCategories");
        }
    }
}
