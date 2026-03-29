using FinTrack.Core.DTOs;
using FinTrack.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Api.Responses;
using FluentValidation;
using FinTrack.Core.Validations;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly CrearCategoryValidator _crearValidator;
        private readonly ActualizarCategoryValidator _actualizarValidator;

        public CategoryController(ICategoryService categoryService, CrearCategoryValidator crearValidator, ActualizarCategoryValidator actualizarValidator)
        {
            _categoryService = categoryService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpGet("dto")]
        public async Task<IActionResult> GetCategoriesDto()
        {
            var categoriesDto = await _categoryService.GetCategoriesAsync();
            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto);
            return Ok(response);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertCategoryDto([FromBody] CategoryDto categoryDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var createdCategoryDto = await _categoryService.InsertCategoryAsync(categoryDto);
                var response = new ApiResponse<CategoryDto>(createdCategoryDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la categoría", error = ex.Message });
            }
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateCategoryDto(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id) 
                return BadRequest("El ID de la categoría no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                if (!updated) 
                    return NotFound("Categoría no encontrada.");

                return Ok(new ApiResponse<bool>(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la categoría", error = ex.Message });
            }
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteCategoryDto(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted) 
                return NotFound("Categoría no encontrada.");

            return Ok(new ApiResponse<bool>(true));
        }
    }
}
