using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("dto")]
        public async Task<IActionResult> GetCategoriesDto()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Ok(categoriesDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertCategoryDto(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                IsActive = 1
            };
            await _categoryRepository.InsertCategoryAsync(category);
            categoryDto.Id = category.Id;
            return Ok(categoryDto);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateCategoryDto(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id) return BadRequest("ID no coincide");

            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _categoryRepository.UpdateCategoryAsync(category);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteCategoryDto(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
