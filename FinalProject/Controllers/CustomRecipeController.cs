using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    /// <summary>
    /// This controller handles CRUD operations for custom recipes, allowing users to create, retrieve, update, and delete recipes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomRecipeController : ControllerBase
    {
        private ICustomRecipeRepository _customRecipeRepository;

        /// <summary>
        /// Constructor for CustomRecipeController. Injects the ICustomRecipeRepository service for managing recipe data.
        /// </summary>
        /// <param name="customRecipeRepository"></param>
        public CustomRecipeController(ICustomRecipeRepository customRecipeRepository)
        {
            _customRecipeRepository = customRecipeRepository;
        }

        /// <summary>
        /// Function: GetCustomRecipes
        /// Purpose: Retrieves a list of all custom recipes from the repository.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing a list of recipes or an empty list if none are found.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomRecipes()
        {
            var recipes = new List<Recipe>();
            try
            {

                recipes = await _customRecipeRepository.GetCustomRecipes();

            } catch(Exception ex)
            {

            }

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

        /// <summary>
        /// Function: GetCustomRecipe
        /// Purpose: Retrieves a single custom recipe by its ID.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing the requested recipe or a NotFound response if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomRecipe(int id)
        {
            var recipe = await _customRecipeRepository.GetCustomRecipe(id);

            if (recipe == null)
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

        /// <summary>
        /// Function: Create
        /// Purpose: Creates a new custom recipe with the provided data from RecipeDto.
        /// Return Type: Task<IActionResult> - An asynchronous action result with a success message upon creation, or BadRequest if the model state is invalid.
        /// </summary>
        /// <param name="recipeDto"></param>
        /// <returns></returns>
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

            await _customRecipeRepository.CreateRecipe(recipe);

            return Ok(new { Message = "New recipe has been Added Successfully!!" });
        }

        /// <summary>
        /// Function: Update
        /// Purpose: Updates an existing custom recipe with the data provided in RecipeDto.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing the updated recipe, or NotFound if the recipe is not found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipeDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RecipeDto recipeDto)
        {

            var recipe = await _customRecipeRepository.GetCustomRecipe(id);

            if (recipe == null)
                return NotFound();

            recipe.Title = recipeDto.Title;
            recipe.Ingredients = recipeDto.Ingredients;
            recipe.Instructions = recipeDto.Instructions;
            recipe.ImageUrl = recipeDto.ImageUrl;
            recipe.Source = recipeDto.Source;

            await _customRecipeRepository.UpdateRecipe(recipe);

            return Ok(recipe);
        }

        /// <summary>
        /// Function: Delete
        /// Purpose: Deletes a custom recipe by its ID.
        /// Return Type: Task<IActionResult> - An asynchronous action result with a success message upon deletion.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customRecipeRepository.DeleteRecipe(id);
            return Ok("Recipe has been successfully Deleted!!");
        }
    }
}
