using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.CustomEntities;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Exceptions;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly CrearCategoryDtoValidator _crearValidator;
        private readonly ActualizarCategoryDtoValidator _actualizarValidator;

        public CategoryController(
            IMapper mapper,
            ICategoryService categoryService,
            CrearCategoryDtoValidator crearValidator,
            ActualizarCategoryDtoValidator actualizarValidator)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        /// <summary>
        /// Recupera una lista paginada de categorías de transacciones como objetos de transferencia de datos (DTO) según los filtros especificados.
        /// </summary>
        /// <remarks>Este método utiliza un mapeador para convertir las categorías recuperadas en DTO, que luego se 
        /// devuelven junto con la información de paginación. Si se produce un error durante el proceso, se devuelve un 
        /// código de estado 500 con los detalles del error.<see cref="ApiResponse{T}"/></remarks>
        /// <param name="filters">Los filtros aplicados para buscar categorías, como el nombre de la categoría y la paginación.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="CategoryDto"/> 
        /// y detalles de paginación.</returns>
        /// <response code="200">Retorna la lista de [CategoryDto]</response>
        /// <response code="400">Petición incorrecta o filtros inválidos</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<CategoryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetCategoriesDtoMapper([FromQuery] CategoryQueryFilter filters)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(filters);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories.Pagination);

            var pagination = new Pagination
            {
                TotalCount = categories.Pagination.TotalCount,
                PageSize = categories.Pagination.PageSize,
                CurrentPage = categories.Pagination.CurrentPage,
                TotalPages = categories.Pagination.TotalPages,
                HasNextPage = categories.Pagination.HasNextPage,
                HasPreviousPage = categories.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto)
            {
                Pagination = pagination,
                Messages = categories.Messages
            };

            return StatusCode((int)categories.StatusCode, response);
        }

        /// <summary>
        /// Recupera el detalle de una categoría específica por su identificador único.
        /// </summary>
        /// <remarks>Busca la categoría en la base de datos. En caso de no ser encontrada, se arroja una excepción de negocio 404 de inmediato.</remarks>
        /// <param name="id">Identificador único de la categoría.</param>
        /// <returns>Un <see cref="ApiResponse{T}"/> con el objeto <see cref="CategoryDto"/> encontrado.</returns>
        /// <response code="200">Retorna la categoría solicitada</response>
        /// <response code="404">Categoría no encontrada</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<CategoryDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        /// <summary>
        /// Recupera una lista paginada de categorías utilizando consultas optimizadas con Dapper.
        /// </summary>
        /// <remarks>Diseñado para acelerar los tiempos de respuesta en listados de categorías dentro de la aplicación móvil.</remarks>
        /// <param name="filters">Filtros de búsqueda y de paginación de la consulta.</param>
        /// <returns>Colección paginada estandarizada de <see cref="CategoryDto"/>.</returns>
        /// <response code="200">Retorna la lista de categorías obtenida mediante Dapper</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<CategoryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetCategoriesDtoMapperDapper([FromQuery] CategoryQueryFilter filters)
        {
            var categories = await _categoryService.GetAllCategoriesDapperAsync(filters);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories.Pagination);

            var pagination = new Pagination
            {
                TotalCount = categories.Pagination.TotalCount,
                PageSize = categories.Pagination.PageSize,
                CurrentPage = categories.Pagination.CurrentPage,
                TotalPages = categories.Pagination.TotalPages,
                HasNextPage = categories.Pagination.HasNextPage,
                HasPreviousPage = categories.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<CategoryDto>>(categoriesDto)
            {
                Pagination = pagination,
                Messages = categories.Messages
            };

            return StatusCode((int)categories.StatusCode, response);
        }

        /// <summary>
        /// Añade una nueva categoría para la organización de las finanzas del usuario.
        /// </summary>
        /// <remarks>El objeto pasa por FluentValidation antes de procesar el guardado final en los almacenes de datos.</remarks>
        /// <param name="categoryDto">Objeto que transfiere los datos correspondientes a la nueva categoría.</param>
        /// <returns>El objeto de la categoría creada con su ID generado.</returns>
        /// <response code="200">Categoría añadida exitosamente al catálogo</response>
        /// <response code="400">Error en las validaciones de los campos entregados</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Category>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertCategoryDtoMapper(CategoryDto categoryDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = _mapper.Map<Category>(categoryDto);
            await _categoryService.InsertCategory(category);
            var response = new ApiResponse<Category>(category);
            return Ok(response);
        }

        /// <summary>
        /// Modifica por completo las propiedades de una categoría existente.
        /// </summary>
        /// <remarks>Controla la integridad del ID entre la URL y el DTO, valida que exista y actualiza el registro.</remarks>
        /// <param name="id">ID de la categoría que se desea actualizar.</param>
        /// <param name="categoryDto">Datos modificados del objeto.</param>
        /// <returns>La categoría actualizada con los nuevos datos consolidados.</returns>
        /// <response code="200">Categoría modificada correctamente</response>
        /// <response code="400">El ID de la categoría no coincide con la ruta</response>
        /// <response code="404">Categoría no encontrada para proceder al cambio</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Category>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

            _mapper.Map(categoryDto, category);
            _categoryService.UpdateCategory(category);
            var response = new ApiResponse<Category>(category);
            return Ok(response);
        }

        /// <summary>
        /// Elimina de forma lógica o física una categoría asociada a un usuario específico.
        /// </summary>
        /// <param name="id">ID único de la categoría a dar de baja.</param>
        /// <param name="userId">ID del usuario dueño de la categoría para resguardar la seguridad de la acción.</param>
        /// <returns>Un booleano indicando el éxito del borrado de la categoría.</returns>
        /// <response code="200">Categoría dada de baja de manera exitosa</response>
        /// <response code="404">Categoría no encontrada en el sistema</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteCategoryDtoMapper(int id, int userId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                throw new BusinessException("Categoría no encontrada.", HttpStatusCode.NotFound);

            await _categoryService.DeleteCategory(id, userId);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }
        #endregion
    }
}
