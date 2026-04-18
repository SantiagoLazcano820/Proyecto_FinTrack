using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface ICategoryRepository 
    {
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
