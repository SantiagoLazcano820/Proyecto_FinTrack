using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.Interfaces;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using System.Globalization;
using System.Net;

namespace FinTrack.Services.Services
{
    public class UserService : IUserService
    {
        //private readonly IBaseRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        //public UserService(IBaseRepository<User> userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(UserQueryFilter filters)
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    users = users.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(filters.LastName))
                {
                    users = users.Where(x => x.LastName.ToLower().Contains(filters.LastName.ToLower()));
                }
                if (!string.IsNullOrEmpty(filters.Email))
                {
                    users = users.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower()));
                }
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersDapperAsync()
        {
            return await _unitOfWork.UserRepository.GetAllUsersDapperAsync();
        }

        public async Task<User> GetUserByIdDapperAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetUserByIdDapperAsync(id);
        }

        public async Task InsertUser(User user)
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            if (users.Any(u => u.Email.ToLower() == user.Email.ToLower()))
            {
                throw new BusinessException("El correo electrónico ya está registrado.", HttpStatusCode.Conflict);
            }

            string[] dominiosPermitidos = { "@gmail.com", "@outlook.com", "@ucb.edu.bo" };
            if (!dominiosPermitidos.Any(d => user.Email.EndsWith(d)))
            {
                throw new BusinessException("Dominio de correo no permitido.", HttpStatusCode.Forbidden);
            }

            user.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.Name.Trim().ToLower());
            user.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName.Trim().ToLower());
            user.Email = user.Email.Trim().ToLower();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateUser(User user)
        {
            var existingUser = _unitOfWork.UserRepository.GetById(user.Id);
            if (existingUser == null)
            {
                throw new BusinessException("El usuario no existe", HttpStatusCode.BadRequest);
            }

            user.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.Name.Trim().ToLower());
            user.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName.Trim().ToLower());
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return false;
            }

            var transactions = await _unitOfWork.TransactionRepository.GetAll();
            var userTransactions = transactions.Where(t => t.UserId == id);

            foreach (var transaction in userTransactions)
            {
                await _unitOfWork.TransactionRepository.Delete(transaction.Id);
            }

            var categories = await _unitOfWork.CategoryRepository.GetAll();
            var userCategories = categories.Where(c => c.UserId == id);
            foreach (var category in userCategories)
            {
                await _unitOfWork.CategoryRepository.Delete(category.Id);
            }

            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var horaActual = DateTime.Now.Hour;
            if (horaActual >= 2 && horaActual < 4)
            {
                throw new BusinessException("Mantenimiento: Acceso disponible a partir de las 04:00 AM.", HttpStatusCode.ServiceUnavailable);
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmailDapperAsync(email);

            if (user != null)
            {
                if (user.IsActive == 0)
                {
                    throw new BusinessException("La cuenta se encuentra suspendida o inactiva.", HttpStatusCode.Forbidden);
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (isValid)
                {
                    user.Password = null;
                    return user;
                }
            }
            throw new BusinessException("El correo o la contraseña son incorrectos.", HttpStatusCode.Unauthorized);
        }
    }
}