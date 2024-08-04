using JobPortal.Contexts;
using JobPortal.Models;
using JobPortal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JobPortalContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(JobPortalContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
