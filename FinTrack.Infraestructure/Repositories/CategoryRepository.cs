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

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }
    }
}
