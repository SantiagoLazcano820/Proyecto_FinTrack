using FinTrack.Core.Entities;

namespace FinTrack.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task InsertCategory(Category category);
        Task UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
