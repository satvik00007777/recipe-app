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
                var responseData = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, string>>(responseData);

                if(responseObject != null && responseObject.TryGetValue("token", out string token))
                {
                    Response.Cookies.Append("JwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });
                }

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
            TempData["SignupDto"] = JsonSerializer.Serialize(signupDto);

            return RedirectToAction("Preferences", "Auth");
        }

        public IActionResult Preferences() { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPreferences(string[] categories)
        {
            if (categories.Length < 3)
            {
                ViewBag.ErrorMessage = "Please select at least 3 categories.";
                return View("Preferences");
            }

            var signupDtoJson = TempData["SignupDto"] as string;
            if(signupDtoJson == null)
            {
                return RedirectToAction("Signup");
            }

            var signupDto = JsonSerializer.Deserialize<SignupDto>(signupDtoJson);

            signupDto.Preferences = string.Join(",", categories);

            Console.WriteLine(signupDto);

            var jsonContent = new StringContent(JsonSerializer.Serialize(signupDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/Signup", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Recipe");
            }

            ViewBag.ErrorMessage = "Failed to submit preferences.";
            return View("Preferences");
        }
    }
}
