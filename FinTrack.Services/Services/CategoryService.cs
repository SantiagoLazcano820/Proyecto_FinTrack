using FinTrack.Core.Interfaces;
using AutoMapper;
using FinTrack.Core.Entities;
using FinTrack.Core.DTOs;

namespace FinTrack.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public readonly string[] ForbiddenWords =
        {
            "violencia", "odio", "grosería", "discriminación", "pornografía"
        };

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> InsertCategoryAsync(CategoryDto categoryDto)
        {
            if (ContainsForbiddenContent(categoryDto.Name) || ContainsForbiddenContent(categoryDto.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            var existing = await _categoryRepository.GetByNameAsync(categoryDto.Name);
            if (existing != null)
            {
                throw new Exception("El nombre de la categoría ya existe");
            }

            var category = _mapper.Map<Category>(categoryDto);
            category.IsActive = true;
            
            await _categoryRepository.InsertCategoryAsync(category);
            
            categoryDto.Id = category.Id;
            return categoryDto;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return false;

            if (ContainsForbiddenContent(categoryDto.Name) || ContainsForbiddenContent(categoryDto.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            var existing = await _categoryRepository.GetByNameAsync(categoryDto.Name);
            if (existing != null && existing.Id != id)
            {
                throw new Exception("El nombre de la categoría ya existe");
            }

            _mapper.Map(categoryDto, category);

            await _categoryRepository.UpdateCategoryAsync(category);
            return true;
        }

        public bool ContainsForbiddenContent(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            foreach (var word in ForbiddenWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteCategoryAsync(id);
            return true;
        }
    }
}
