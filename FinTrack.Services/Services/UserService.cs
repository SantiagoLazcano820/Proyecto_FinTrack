using FinTrack.Core.Interfaces;
using AutoMapper;
using FinTrack.Core.Entities;
using FinTrack.Core.DTOs;

namespace FinTrack.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public readonly string[] ForbiddenWords =
        {
            "violencia", "odio", "grosería", "discriminación", "pornografía"
        };

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> InsertUserAsync(UserDto userDto)
        {
            if (ContainsForbiddenContent(userDto.Name) || ContainsForbiddenContent(userDto.LastName))
            {
                throw new Exception("El contenido no es permitido");
            }

            var user = _mapper.Map<User>(userDto);
            user.Password = "ChangeMe123!";
            user.IsActive = true;

            await _userRepository.InsertUser(user);

            userDto.Id = user.Id;
            return userDto;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            if (ContainsForbiddenContent(userDto.Name) || ContainsForbiddenContent(userDto.LastName))
            {
                throw new Exception("El contenido no es permitido");
            }

            _mapper.Map(userDto, user);

            await _userRepository.UpdateUser(user);
            return true;
        }

        public bool ContainsForbiddenContent(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            foreach (var word in ForbiddenWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteUser(user);
            return true;
        }
    }
}
