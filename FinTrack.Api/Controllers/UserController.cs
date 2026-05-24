using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.CustomEntities;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CrearUserDtoValidator _crearValidator;
        private readonly ActualizarUserDtoValidator _actualizarValidator;

        public UserController(
            IUserService userService,
            IMapper mapper,
            CrearUserDtoValidator crearValidator,
            ActualizarUserDtoValidator actualizarValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        /// <summary>
        /// Recupera una lista paginada de usuarios como objetos de transferencia de datos (DTO) según los filtros especificados.
        /// </summary>
        /// <remarks>Este método utiliza un mapeador para convertir los usuarios recuperados en DTO, que luego se 
        /// devuelven junto con la información de paginación. Si se produce un error durante el proceso, se devuelve un 
        /// código de estado 500 con los detalles del error.<see cref="ApiResponse{T}"/></remarks>
        /// <param name="filters">Los filtros que se aplicarán al recuperar usuarios, como la paginación y criterios de búsqueda por nombre o correo.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="UserDto"/> 
        /// y detalles de paginación.</returns>
        /// <response code="200">Retorna la lista de [UserDto]</response>
        /// <response code="400">Petición incorrecta o filtros inválidos</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetUsersDtoMapper([FromQuery] UserQueryFilter filters)
        {
            var users = await _userService.GetAllUsersAsync(filters);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users.Pagination);

            var pagination = new Pagination
            {
                TotalCount = users.Pagination.TotalCount,
                PageSize = users.Pagination.PageSize,
                CurrentPage = users.Pagination.CurrentPage,
                TotalPages = users.Pagination.TotalPages,
                HasNextPage = users.Pagination.HasNextPage,
                HasPreviousPage = users.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto)
            {
                Pagination = pagination,
                Messages = users.Messages
            };

            return StatusCode((int)users.StatusCode, response);
        }

        /// <summary>
        /// Recupera el detalle de un usuario específico por su identificador único.
        /// </summary>
        /// <remarks>Busca en la base de datos el usuario solicitado. Si no existe, se arroja una excepción de negocio con código 404.</remarks>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Un <see cref="ApiResponse{T}"/> con el objeto <see cref="UserDto"/> encontrado.</returns>
        /// <response code="200">Retorna el usuario solicitado</response>
        /// <response code="404">Usuario no encontrado</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetUserByIdDtoMapper(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("Usuario no encontrado.", HttpStatusCode.NotFound);

            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Recupera una lista paginada de usuarios utilizando Dapper para optimizar el rendimiento.
        /// </summary>
        /// <remarks>Ideal para consultas de lectura de alta velocidad directa en la base de datos. Devuelve la estructura paginada estándar.</remarks>
        /// <param name="filters">Filtros de búsqueda y paginación para la consulta optimizada.</param>
        /// <returns>Colección paginada de objetos <see cref="UserDto"/>.</returns>
        /// <response code="200">Retorna la lista de usuarios obtenida con Dapper</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetUsersDtoMapperDapper([FromQuery] UserQueryFilter filters)
        {
            var users = await _userService.GetAllUsersDapperAsync(filters);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users.Pagination);

            var pagination = new Pagination
            {
                TotalCount = users.Pagination.TotalCount,
                PageSize = users.Pagination.PageSize,
                CurrentPage = users.Pagination.CurrentPage,
                TotalPages = users.Pagination.TotalPages,
                HasNextPage = users.Pagination.HasNextPage,
                HasPreviousPage = users.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto)
            {
                Pagination = pagination,
                Messages = users.Messages
            };

            return StatusCode((int)users.StatusCode, response);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <remarks>Valida que los campos obligatorios cumplan con las reglas de negocio a través de FluentValidation antes de persistir los datos.</remarks>
        /// <param name="userDto">Objeto que contiene la información detallada del usuario a crear.</param>
        /// <returns>El objeto del usuario creado junto con su ID asignado por la base de datos.</returns>
        /// <response code="200">Usuario registrado exitosamente</response>
        /// <response code="400">Error de validación en los datos de entrada entregados</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<User>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertUserDtoMapper(UserDto userDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(userDto);
            await _userService.InsertUser(user);
            var response = new ApiResponse<User>(user);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza por completo el perfil de un usuario existente.
        /// </summary>
        /// <remarks>Verifica que el ID de la URL coincida con el del modelo, ejecuta las validaciones correspondientes y aplica las modificaciones.</remarks>
        /// <param name="id">ID único del usuario a modificar.</param>
        /// <param name="userDto">Datos modificados que se asignarán al registro.</param>
        /// <returns>El objeto del usuario con todos sus cambios aplicados.</returns>
        /// <response code="200">Modificación del usuario realizada correctamente</response>
        /// <response code="400">El ID especificado en la ruta no coincide con el cuerpo</response>
        /// <response code="404">El usuario a editar no existe en el sistema</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<User>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateUserDtoMapper(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
                throw new BusinessException("El ID no coincide.", HttpStatusCode.BadRequest);

            var validationResult = await _actualizarValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("Usuario no encontrado para actualización.", HttpStatusCode.NotFound);
            _mapper.Map(userDto, user);
            _userService.UpdateUser(user);
            var response = new ApiResponse<User>(user);
            return Ok(response);
        }

        /// <summary>
        /// Elimina un usuario del sistema de forma permanente por su ID.
        /// </summary>
        /// <param name="id">ID de la cuenta de usuario a remover.</param>
        /// <returns>Un valor booleano indicando el éxito completo del borrado.</returns>
        /// <response code="200">Usuario eliminado con éxito del sistema</response>
        /// <response code="404">El usuario indicado no fue encontrado</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUserDtoMapper(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("Usuario no encontrado para eliminar.", HttpStatusCode.NotFound);

            await _userService.DeleteUser(id);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }
        #endregion
    }
}