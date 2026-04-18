using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;
using BCrypt.Net;

namespace FinTrack.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task InsertUser(User user)
        {
            // 1. RN-03: Validar duplicados
            var users = await _userRepository.GetAll();
            if (users.Any(u => u.Email == user.Email))
            {
                throw new Exception("El correo electrónico ya está registrado.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _userRepository.Insert(user);
        }

        public async Task UpdateUser(User user)
        {
            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null)
            {
                throw new Exception("El usuario no existe.");
            }

            _userRepository.Update(user);
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return false;

            await _userRepository.Delete(id);
            return true;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (isValid) return user;
            }
            return null;
        }
    }
}