using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomRecipeController : ControllerBase
    {
        private readonly FinalProjectDbContext _context;

        public CustomRecipeController(FinalProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomRecipes()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomRecipe()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeDto recipeDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = new Recipe
            {
                Title = recipeDto.Title,
                Ingredients = recipeDto.Ingredients,
                Instructions = recipeDto.Instructions,
                ImageUrl = recipeDto.ImageUrl,
                Source = recipeDto.Source,
                UserId = null
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "New recipe has been Added Successfully!!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }
    }
}
