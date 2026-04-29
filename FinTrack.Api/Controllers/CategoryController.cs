using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> GetCategoriesDtoMapper([FromQuery] CategoryQueryFilter filters)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(filters);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetCategoryByIdDtoMapper(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                throw new BusinessException("Categoría no encontrada.", HttpStatusCode.NotFound);

            var categoryDto = _mapper.Map<CategoryDto>(category);
            var response = new ApiResponse<CategoryDto>(categoryDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetCategoriesDtoMapperDapper()
        {
            var categories = await _categoryService.GetAllCategoriesDapperAsync(); 
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/dapper/{id}")]
        public async Task<IActionResult> GetCategoryByIdDtoMapperDapper(int id)
        {
            var category = await _categoryService.GetCategoryByIdDapperAsync(id);
            if (category == null)
                throw new BusinessException("Categoría no encontrada.", HttpStatusCode.NotFound);

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
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _categoryService.InsertCategory(category);
                var response = new ApiResponse<Category>(category);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al crear la categoría.", ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateCategoryDtoMapper(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                throw new BusinessException("El ID de la categoría no coincide.", HttpStatusCode.BadRequest);

            var validationResult = await _actualizarValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                throw new BusinessException("Categoría no encontrada.", HttpStatusCode.NotFound);

            try
            {
                _mapper.Map(categoryDto, category);
                _categoryService.UpdateCategory(category);
                var response = new ApiResponse<Category>(category);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al actualizar la categoría.", ex);
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteCategoryDtoMapper(int id, int userId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                throw new BusinessException("Categoría no encontrada.", HttpStatusCode.NotFound);

            try
            {
                await _categoryService.DeleteCategory(id, userId);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al intentar eliminar la categoría.", ex);
            }
        }
        #endregion
    }
}
