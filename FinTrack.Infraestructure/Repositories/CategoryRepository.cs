using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infraestructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinTrackContext _context;

        public CategoryRepository(FinTrackContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
