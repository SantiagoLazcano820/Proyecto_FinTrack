using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User/dto
        [HttpGet("dto")]
        public async Task<IActionResult> GetUsersDto()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var usersDto = users.Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email
            });

            return Ok(usersDto);
        }

        // GET: api/User/dto/5
        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetUserByIdDto(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            };

            return Ok(userDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertUserDto(UserDto userDto)
        {
            var user = new User
            {
                RoleId = userDto.RoleId,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = "ChangeMe123!",
                IsActive = 1
            };

            await _userRepository.InsertUser(user);

            userDto.Id = user.Id;
            return CreatedAtAction(nameof(GetUserByIdDto), new { id = userDto.Id }, userDto);
        }

        // PUT: api/User/dto/5
        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateUserDto(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("El ID no coincide.");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no existente.");

            user.RoleId = userDto.RoleId;
            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;

            await _userRepository.UpdateUser(user);
            return NoContent();
        }

        // DELETE: api/User/dto/5
        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteUserDto(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUser(user);
            return NoContent();
        }
    }
}