using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> _categoryRepository;

        public CategoryService(IBaseRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task InsertCategory(Category category)
        {
            var allCategories = await _categoryRepository.GetAll();
            var isDuplicate = allCategories.Any(c => c.Name.ToLower() == category.Name.ToLower());

            if (isDuplicate)
            {
                throw new Exception($"La categoría '{category.Name}' ya existe en el sistema.");
            }

            await _categoryRepository.Insert(category);
        }

        public async Task UpdateCategory(Category category)
        {
            var existing = await _categoryRepository.GetById(category.Id);
            if (existing == null) throw new Exception("La categoría no existe.");

            _categoryRepository.Update(category);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            await _categoryRepository.Delete(id);
            return true;
        }
    }
}