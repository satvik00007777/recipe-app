using FinalProject.DTOs;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    /// <summary>
    /// This controller handles operations related to retrieving custom recipes specific to a user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MyRecipeController : ControllerBase
    {
        private IMyRecipeRepository _myRecipeRepository;

        /// <summary>
        /// Constructor for MyRecipeController. Injects the IMyRecipeRepository service for managing recipe data specific to a user.
        /// </summary>
        /// <param name="myRecipeRepository"></param>
        public MyRecipeController(IMyRecipeRepository myRecipeRepository)
        {
            _myRecipeRepository = myRecipeRepository;
        }

        /// <summary>
        /// Function: GetCustomRecipesByUser
        /// Purpose: Retrieves a list of custom recipes created by a specific user, identified by user ID.
        /// Return Type: Task<IActionResult> - An asynchronous action result containing a list of the user's custom recipes.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCustomRecipesByUser(int userId)
        {
            var recipes = await _myRecipeRepository.GetCustomRecipesByUser(userId);

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
