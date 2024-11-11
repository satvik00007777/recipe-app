using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public interface ICustomRecipeRepository
    {
        Task<List<Recipe>> GetCustomRecipes();
        Task<Recipe> GetCustomRecipe(int id);
        Task CreateRecipe(Recipe recipe);
        Task UpdateRecipe(Recipe recipe);
        Task DeleteRecipe(int id);
    }

    public class CustomRecipeRepository : ICustomRecipeRepository
    {
        private readonly FinalProjectDbContext _context;

        public CustomRecipeRepository(FinalProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetCustomRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task<Recipe> GetCustomRecipe(int id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async Task CreateRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }
    }
}
