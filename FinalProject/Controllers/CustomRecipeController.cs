using FinalProject.DTOs;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //public IActionResult GetUserInfo()
        //{
        //    var userId = User.FindFirst("id")?.Value;

        //    return Ok(new { UserId = userId });
        //}

        [HttpGet]
        public async Task<IActionResult> GetCustomRecipes()
        {
            var recipes = new List<Recipe>();
            try
            {


                recipes = await _context.Recipes.ToListAsync();
            } catch(Exception ex)
            {

            }

            //var userId = GetUserInfo();

            var recipesList = recipes.Select(r => new RecipeDto
            {
                Title = r.Title,
                Ingredients = r.Ingredients,
                Instructions = r.Instructions,
                ImageUrl = r.ImageUrl ?? string.Empty,
                Source = r.Source ?? "Custom",
                RecipeId = r.RecipeId
            }).ToList();

            return Ok(recipesList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if(recipe == null)
                return NotFound();

            var singleRecipe = new RecipeDto
            {
                Title = recipe.Title,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions,
                ImageUrl = recipe.ImageUrl ?? string.Empty,
                Source = recipe.Source ?? "Custom",
                RecipeId = recipe.RecipeId
            };

            return Ok(singleRecipe);
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
        public async Task<IActionResult> Update(int id, RecipeDto recipeDto)
        {
            
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
                return NotFound();

            recipe.Title = recipeDto.Title;
            recipe.Ingredients = recipeDto.Ingredients;
            recipe.Instructions = recipeDto.Instructions;
            recipe.ImageUrl = recipeDto.ImageUrl;
            recipe.Source = recipeDto.Source;
            //recipe.RecipeId = recipeDto.RecipeId;

            await _context.SaveChangesAsync();

            return Ok(recipe);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
                return NotFound();

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok("Recipe has been successfully Deleted!!");
        }
    }
}
