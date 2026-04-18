using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Services;
using FinTrack.Services.Validators;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetUsersDtoMapper()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetUserByIdDtoMapper(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado.");

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
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            try
            {
                var user = _mapper.Map<User>(userDto);
                await _userService.InsertUser(user);
                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateUserDtoMapper(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id) 
                return BadRequest("El ID no coincide");

            var validationResult = await _actualizarValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User no encontrado.");
            try
            {
                _mapper.Map(userDto, user);
                await _userService.UpdateUser(user);
                var response = new ApiResponse<User>(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteUserDtoMapper(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User no encontrado.");

            try
            {
                await _userService.DeleteUser(id);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar", error = ex.Message });
            }
        }

        [HttpPost("dto/mapper/login/")]
        public async Task<IActionResult> Login([FromBody] UserDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { message = "Email y contraseña requeridos" });
            }
            try
            {
                var user = await _userService.Authenticate(loginDto.Email, loginDto.Password!);
                if (user == null)
                    return Unauthorized(new { message = "Credenciales incorrectas" });

                var userDto = _mapper.Map<UserDto>(user);
                var response = new ApiResponse<UserDto>(userDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message });
            }
        }
        #endregion
    }
}