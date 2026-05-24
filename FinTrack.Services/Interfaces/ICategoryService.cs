using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseData> GetAllCategoriesAsync(CategoryQueryFilter filters);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<ResponseData> GetAllCategoriesDapperAsync(CategoryQueryFilter filters);
        Task InsertCategory(Category category);
        void UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id, int userId);
    }
}
