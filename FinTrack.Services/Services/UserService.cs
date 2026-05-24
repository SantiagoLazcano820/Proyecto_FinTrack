using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters)
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
            var pagedUsers = PagedList<object>.Create(users, filters.PageNumber, filters.PageSize);

            if (pagedUsers.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Registros de usuarios recuperados correctamente" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.warning.ToString(), Description = "No fue posible recuperar la cantidad de registros de usuarios" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task<ResponseData> GetAllUsersDapperAsync(UserQueryFilter filters)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersDapperAsync();
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
            var pagedUsers = PagedList<object>.Create(users, filters.PageNumber, filters.PageSize);

            if (pagedUsers.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Registros de usuarios recuperados correctamente" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.warning.ToString(), Description = "No fue posible recuperar la cantidad de registros de usuarios" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task InsertUser(User user)
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            if (users.Any(u => u.Email.ToLower() == user.Email.ToLower()))
            {
                var errMessage = "El correo electrónico ya está registrado.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.Conflict);
            }

            string[] dominiosPermitidos = { "@gmail.com", "@outlook.com", "@ucb.edu.bo" };
            if (!dominiosPermitidos.Any(d => user.Email.EndsWith(d)))
            {
                var errMessage = "Dominio de correo no permitido.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.Forbidden);
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
                var errMessage = "El usuario no existe";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.BadRequest);
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
    }
}