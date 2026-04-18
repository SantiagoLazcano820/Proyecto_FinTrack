using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infraestructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(FinTrackContext context) : base(context)
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _entities.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
