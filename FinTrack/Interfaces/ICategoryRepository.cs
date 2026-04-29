using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesByUserIdDapperAsync(int userId);
        Task<IEnumerable<Category>> GetAllCategoriesDapperAsync();
        Task<Category> GetCategoryByIdDapperAsync(int id);
    }
}
