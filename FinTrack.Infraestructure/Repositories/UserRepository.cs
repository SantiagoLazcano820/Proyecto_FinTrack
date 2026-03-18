using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbFinTrackContext _context;
        public UserRepository(DbFinTrackContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(x => x.Role).ToListAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task InsertUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
