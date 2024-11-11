using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public interface IAccountRepository
    {
        public Task<User> GetUersByUserName(string userName);
        Task SaveChanges();
        Task AddUsers(User newUser);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly FinalProjectDbContext _context;
        public AccountRepository(FinalProjectDbContext finalProjectDbContext) 
        {
            _context = finalProjectDbContext;
        }
        public async Task<User> GetUersByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddUsers(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
