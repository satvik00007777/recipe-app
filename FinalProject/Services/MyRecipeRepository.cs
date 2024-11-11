using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public interface IMyRecipeRepository
    {
        Task<List<Recipe>> GetCustomRecipesByUser(int userId);
    }

    public class MyRecipeRepository : IMyRecipeRepository
    {
        private readonly FinalProjectDbContext _context;

        public MyRecipeRepository(FinalProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetCustomRecipesByUser(int userId)
        {
            return await _context.Recipes
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
