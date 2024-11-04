using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class CustomRecipeController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public CustomRecipeController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IActionResult> Index()
        {
            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.GetAsync("https://localhost:7255/api/CustomRecipe");

            if(response.IsSuccessStatusCode)
            {
                var recipes = await response.Content.ReadFromJsonAsync<List<CustomRecipeDto>>();
                return View(recipes);
            }

            return View(new List<CustomRecipeDto>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomRecipeDto customRecipeDto)
        {
            if(!ModelState.IsValid)
            {
                return View(customRecipeDto);
            }

            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.PostAsJsonAsync("https://localhost:7255/api/CustomRecipe", customRecipeDto);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(customRecipeDto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.GetAsync($"https://localhost:7255/api/customrecipe/{id}");

            if (response.IsSuccessStatusCode)
            {
                var recipe = await response.Content.ReadFromJsonAsync<CustomRecipeDto>();
                return View(recipe);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CustomRecipeDto customRecipeDto)
        {
            if(!ModelState.IsValid)
            {
                return View(customRecipeDto);
            }

            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.PutAsJsonAsync($"https://localhost:7255/api/customrecipe/{id}", customRecipeDto);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "CustomRecipe");
            }

            return View(customRecipeDto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.GetAsync($"https://localhost:7255/api/customrecipe/{id}");

            if(response.IsSuccessStatusCode)
            {
                var recipe = await response.Content.ReadFromJsonAsync<CustomRecipeDto>();
                return View(recipe);
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _apiClientService.GetAuthenticatedClient();
            var response = await client.DeleteAsync($"https://localhost:7255/api/customrecipe/{id}");

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
