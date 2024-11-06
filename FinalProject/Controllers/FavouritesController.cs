using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Ensure only authenticated users can add to favorites
    public class FavoritesController : ControllerBase
    {
        // Temporary storage for favorite recipes (to be replaced with a real database or service)
        private static List<FavoriteRecipeDto> _favorites = new List<FavoriteRecipeDto>();

        // Endpoint to get the list of favorite recipes
        [HttpGet]
        public IActionResult GetFavorites()
        {
            return Ok(_favorites);
        }

        // Endpoint to add a recipe to the favorites list
        [HttpPost("add")]
        public IActionResult AddToFavorites([FromBody] FavoriteRecipeDto favoriteRecipe)
        {
            // Check if the recipe already exists in the favorites list (optional check)
            if (_favorites.Any(f => f.UniqueId == favoriteRecipe.UniqueId))
            {
                return BadRequest("Recipe is already in your favorites.");
            }

            _favorites.Add(favoriteRecipe);
            return Ok("Recipe added to favorites.");
        }
    }

    // DTO for Favorite Recipe
    public class FavoriteRecipeDto
    {
        public string UniqueId { get; set; }  // Unique identifier based on the recipe
        public string Title { get; set; }
        public string SourceUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
