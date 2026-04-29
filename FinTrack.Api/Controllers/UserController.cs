using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Services;
using FinTrack.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CrearUserDtoValidator _crearValidator;
        private readonly LoginUserDtoValidator _loginValidator;
        private readonly ActualizarUserDtoValidator _actualizarValidator;

        public UserController(
            IUserService userService,
            IMapper mapper,
            CrearUserDtoValidator crearValidator,
            LoginUserDtoValidator loginValidator,
            ActualizarUserDtoValidator actualizarValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _loginValidator = loginValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetUsersDtoMapper([FromQuery] UserQueryFilter filters)
        {
            var users = await _userService.GetAllUsersAsync(filters);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }

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

        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetUsersDtoMapperDapper()
        {
            var users = await _userService.GetAllUsersDapperAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users); 
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/dapper/{id}")]
        public async Task<IActionResult> GetUserByIdDtoMapperDapper(int id)
        {
            var user = await _userService.GetUserByIdDapperAsync(id);
            if (user == null)
                throw new BusinessException("Usuario no encontrado.", HttpStatusCode.NotFound);

            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertUserDtoMapper(UserDto userDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var user = _mapper.Map<User>(userDto);
                await _userService.InsertUser(user);
                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al registrar el usuario.", ex);
            }
        }

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
            try
            {
                _mapper.Map(userDto, user);
                _userService.UpdateUser(user);
                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al actualizar el usuario.", ex);
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUserDtoMapper(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("Usuario no encontrado para eliminar.", HttpStatusCode.NotFound);

            try
            {
                await _userService.DeleteUser(id);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al intentar eliminar el usuario.", ex);
            }
        }

        [HttpPost("dto/mapper/login/")]
        public async Task<IActionResult> Login([FromBody] UserDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var user = await _userService.Authenticate(loginDto.Email, loginDto.Password!);
                var userDto = _mapper.Map<UserDto>(user);
                var response = new ApiResponse<UserDto>(userDto);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno durante el proceso de autenticación.", ex);
            }
        }
        #endregion
    }
}