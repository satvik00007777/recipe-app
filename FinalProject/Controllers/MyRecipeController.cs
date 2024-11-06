using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyRecipeController : ControllerBase
    {
        private FinalProjectDbContext _context;

        public MyRecipeController(FinalProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCustomRecipesByUser(int userId)
        {
            // Filter recipes by the given userId
            var recipes = await _context.Recipes
                .Where(r => r.UserId == userId)
                .ToListAsync();

            // Map the filtered recipes to RecipeDto
            var recipesList = recipes.Select(r => new RecipeDto
            {
                RecipeId = r.RecipeId,
                Title = r.Title,
                Ingredients = r.Ingredients,
                Instructions = r.Instructions,
                ImageUrl = r.ImageUrl ?? string.Empty,
                Source = r.Source ?? "Custom"
            }).ToList();

            return Ok(recipesList);
        }
    }
}
