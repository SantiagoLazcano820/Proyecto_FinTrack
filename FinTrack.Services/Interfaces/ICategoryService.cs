using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(CategoryQueryFilter filters);
        Task<Category> GetCategoryByIdAsync(int id);
        Task InsertCategory(Category category);
        void UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id, int userId);
    }
}
