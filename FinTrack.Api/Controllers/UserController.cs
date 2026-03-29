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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly CrearUserValidator _crearValidator;
        private readonly ActualizarUserValidator _actualizarValidator;

        public UserController(IUserService userService, CrearUserValidator crearValidator, ActualizarUserValidator actualizarValidator)
        {
            _userService = userService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        // GET: api/User/dto
        [HttpGet("dto")]
        public async Task<IActionResult> GetUsersDto()
        {
            var usersDto = await _userService.GetUsersAsync();
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto);
            return Ok(response);
        }

        // GET: api/User/dto/5
        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetUserByIdDto(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertUserDto([FromBody] UserDto userDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(userDto);
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
                var createdUserDto = await _userService.InsertUserAsync(userDto);
                var response = new ApiResponse<UserDto>(createdUserDto);
                return CreatedAtAction(nameof(GetUserByIdDto), new { id = createdUserDto.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el usuario", error = ex.Message });
            }
        }

        // PUT: api/User/dto/5
        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateUserDto(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("El ID del usuario no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(userDto);
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
                var updated = await _userService.UpdateUserAsync(id, userDto);
                if (!updated)
                    return NotFound("Usuario no existente.");

                return Ok(new ApiResponse<bool>(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el usuario", error = ex.Message });
            }
        }

        // DELETE: api/User/dto/5
        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteUserDto(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(new ApiResponse<bool>(true));
        }
    }
}