using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Exceptions;
using FinTrack.Core.Interfaces;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace FinTrack.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllCategoriesAsync(CategoryQueryFilter filters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAll();
            if (filters != null)
            {
                if (filters.UserId != null)
                {
                    categories = categories.Where(x => x.UserId == filters.UserId);
                }
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    categories = categories.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower()));
                }
            }
            var pagedCategories = PagedList<object>.Create(categories, filters.PageNumber, filters.PageSize);

            if (pagedCategories.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Categorías recuperadas correctamente" } },
                    Pagination = pagedCategories,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.warning.ToString(), Description = "No se encontraron categorías con los filtros aplicados" } },
                    Pagination = pagedCategories,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.CategoryRepository.GetById(id);
        }

        public async Task<ResponseData> GetAllCategoriesDapperAsync(CategoryQueryFilter filters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesDapperAsync();
            if (filters != null)
            {
                if (filters.UserId != null)
                {
                    categories = categories.Where(x => x.UserId == filters.UserId);
                }
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    categories = categories.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower()));
                }
            }
            var pagedCategories = PagedList<object>.Create(categories, filters.PageNumber, filters.PageSize);

            if (pagedCategories.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Categorías recuperadas correctamente" } },
                    Pagination = pagedCategories,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.warning.ToString(), Description = "No se encontraron categorías con los filtros aplicados" } },
                    Pagination = pagedCategories,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task InsertCategory(Category category)
        {
            var userCategories = await _unitOfWork.CategoryRepository.GetCategoriesByUserIdDapperAsync(category.UserId);

            if (userCategories.Count() >= 15)
            {
                var errMessage = "Límite alcanzado: Máximo 15 categorías activas.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.Conflict);
            }

            var isDuplicate = userCategories.Any(c => c.Name.ToLower() == category.Name.ToLower());

            if (isDuplicate)
            {
                var errMessage = $"Ya tienes una categoría llamada '{category.Name}'.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.BadRequest);
            }

            await _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateCategory(Category category)
        {
            var existing = _unitOfWork.CategoryRepository.GetById(category.Id);
            if (existing == null)
            {
                var errMessage = "La Categoria no existe";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.BadRequest);
            }
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategory(int id, int userId)
        {
            var existing = await _unitOfWork.CategoryRepository.GetById(id);

            if (existing == null || existing.UserId != userId)
            {
                var errMessage = "No tienes permiso para eliminar esta categoría o no existe.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.Forbidden);
            }

            var hasTransactions = (await _unitOfWork.TransactionRepository.GetTransactionsByUserIdDapperAsync(userId))
                                  .Any(t => t.CategoryId == id);

            if (hasTransactions)
            {
                var errMessage = "No se puede eliminar: Esta categoría tiene movimientos registrados.";
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = errMessage } },
                };
                throw new BusinessException(errMessage, HttpStatusCode.BadRequest);
            }

            await _unitOfWork.CategoryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}