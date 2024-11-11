using FinalProject.Models;

namespace FinalProject.Services
{
    public interface IRecipeRepository
    {
        Task<User> FindUser(int userId);
    }

    public class RecipeRepository : IRecipeRepository
    {
        private readonly FinalProjectDbContext _context;
        public RecipeRepository(FinalProjectDbContext finalProjectDbContext)
        {
            _context = finalProjectDbContext;
        }

        public async Task<User> FindUser(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
