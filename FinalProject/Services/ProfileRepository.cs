using FinalProject.Models;

namespace FinalProject.Services
{
    public interface IProfileRepository
    {
        Task<User> GetProfile(int userId);
        Task<bool> UpdateProfile(User user);
    }

    public class ProfileRepository : IProfileRepository
    {
        private readonly FinalProjectDbContext _context;

        public ProfileRepository(FinalProjectDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetProfile(int userId)
        {
            // Fetches the user profile based on userId
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> UpdateProfile(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser == null)
                return false;

            // Update the user's profile information
            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
