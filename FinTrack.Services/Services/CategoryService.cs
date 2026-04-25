using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.Interfaces;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using System.Net;

namespace FinTrack.Services.Services
{
    public class CategoryService : ICategoryService
    {
        //private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        //public CategoryService(IBaseRepository<Category> categoryRepository)
        //{
        //    _categoryRepository = categoryRepository;
        //}

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CategoryQueryFilter filters)
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
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.CategoryRepository.GetById(id);
        }

        public async Task InsertCategory(Category category)
        {
            var userCategories = await _unitOfWork.CategoryRepository.GetCategoriesByUserIdAsync(category.UserId);

            if (userCategories.Count() >= 15)
            {
                throw new BusinessException("Límite alcanzado: Máximo 15 categorías activas.", HttpStatusCode.Conflict);
            }

            var isDuplicate = userCategories.Any(c => c.Name.ToLower() == category.Name.ToLower());

            if (isDuplicate)
            {
                throw new BusinessException($"Ya tienes una categoría llamada '{category.Name}'.", HttpStatusCode.BadRequest);
            }

            await _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateCategory(Category category)
        {
            var existing = _unitOfWork.CategoryRepository.GetById(category.Id);
            if (existing == null)
            {
                throw new BusinessException("La Categoria no existe", HttpStatusCode.BadRequest);
            }
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategory(int id, int userId)
        {
            var existing = await _unitOfWork.CategoryRepository.GetById(id);

            if (existing == null || existing.UserId != userId)
            {
                throw new BusinessException("No tienes permiso para eliminar esta categoría o no existe.", HttpStatusCode.Forbidden);
            }

            var hasTransactions = (await _unitOfWork.TransactionRepository.GetTransactionsByUserIdAsync(userId))
                                  .Any(t => t.CategoryId == id);

            if (hasTransactions)
            {
                throw new BusinessException("No se puede eliminar: Esta categoría tiene movimientos registrados.", HttpStatusCode.BadRequest);
            }

            await _unitOfWork.CategoryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}