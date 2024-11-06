using Microsoft.AspNetCore.Mvc;
using FinalProjectMVC.DTOs;
using FinalProjectMVC.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProjectMVC.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public FavouritesController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _apiClientService.GetAuthenticatedClient();
            var response = await httpClient.GetAsync("favorites"); 
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var favorites = JsonSerializer.Deserialize<List<RecipeDto>>(content);
                return View(favorites);
            }
            return View(new List<RecipeDto>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(string title, string source, string imageUrl)
        {
            var httpClient = _apiClientService.GetAuthenticatedClient();
            var uniqueId = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(title + source));

            var favoriteRecipe = new
            {
                UniqueId = uniqueId,
                Title = title,
                Source = source,
                ImageUrl = imageUrl
            };

            var response = await httpClient.PostAsJsonAsync("favourites/add", favoriteRecipe);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
