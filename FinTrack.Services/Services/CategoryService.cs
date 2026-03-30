using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task InsertCategory(Category category)
        {
            var allCategories = await _categoryRepository.GetCategoriesAsync();
            var isDuplicate = allCategories.Any(c => c.Name.ToLower() == category.Name.ToLower());

            if (isDuplicate)
            {
                throw new Exception($"La categoría '{category.Name}' ya existe en el sistema.");
            }

            await _categoryRepository.InsertCategoryAsync(category);
        }

        public async Task UpdateCategory(Category category)
        {
            var existing = await _categoryRepository.GetCategoryByIdAsync(category.Id);
            if (existing == null) throw new Exception("La categoría no existe.");

            await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return true;
        }
    }
}