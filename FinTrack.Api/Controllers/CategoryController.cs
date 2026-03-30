using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly CrearCategoryDtoValidator _crearValidator;
        private readonly ActualizarCategoryDtoValidator _actualizarValidator;

        public CategoryController(
            IMapper mapper,
            ICategoryService categoryService,
            //ICategoryRepository categoryRepository,
            CrearCategoryDtoValidator crearValidator,
            ActualizarCategoryDtoValidator actualizarValidator)
        {
            //_categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetCategoriesDtoMapper()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetCategoryByIdDtoMapper(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound("Categoría no encontrada.");

            var categoryDto = _mapper.Map<CategoryDto>(category);
            var response = new ApiResponse<CategoryDto>(categoryDto);
            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertCategoryDtoMapper(CategoryDto categoryDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _categoryService.InsertCategory(category);
                var response = new ApiResponse<Category>(category);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la categoría", error = ex.Message });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateCategoryDtoMapper(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest("El ID de la categoría no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound("Categoría no encontrada.");

            try
            {
                _mapper.Map(categoryDto, category);
                await _categoryService.UpdateCategory(category);
                var response = new ApiResponse<Category>(category);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la categoría", error = ex.Message });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteCategoryDtoMapper(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound("Categoría no encontrada.");

            try
            {
                await _categoryService.DeleteCategory(id);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la categoría", error = ex.Message });
            }
        }
        #endregion
    }
}
