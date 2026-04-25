using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(int userId);
    }
}
