using FinalProjectMVC.DTOs;
using Microsoft.AspNetCore.Mvc;

public class FavouritesController : Controller
{
    private readonly HttpClient _httpClient;

    public FavouritesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> AddToFavourites(string uri)
    {
        if(string.IsNullOrEmpty(uri))
        {
            return BadRequest("Invalid recipe URI.");
        }

        var recipe = await FetchRecipeFromUri(uri);

        if(recipe == null)
        {
            return View("Error");
        }

        return View(recipe);
    }

    public async Task<IActionResult> Index(string uri)
    {
        if (string.IsNullOrEmpty(uri))
        {
            return BadRequest("Invalid recipe URI.");
        }

        var recipe = await FetchRecipeFromUri(uri);

        if (recipe == null)
        {
            return View("Error");
        }

        return View(recipe);
    }

    [HttpGet]
    public async Task<RecipeDto> FetchRecipeFromUri(string uri)
    {
        var apiUrl = $"https://localhost:7255/api/Recipe/GetFavourites?uri={uri}";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            //var recipe = JsonSerializer.Deserialize<RecipeDto>(responseBody);

            //return recipe;
        }

        return null;
    }
}
