using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Services.Interfaces;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt
            });
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");

            var userDto = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser(CreateUserDTO createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Password = createUserDto.Password, 
                PhoneNumber = createUserDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userService.CreateUserAsync(user);
            if (createdUser == null)
                return BadRequest("Kullanıcı oluşturulamadı.");

            var userDto = new UserResponseDTO
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                PhoneNumber = createdUser.PhoneNumber,
                CreatedAt = createdUser.CreatedAt
            };

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");

            // Sadece değişen alanları güncelle
            if (updateUserDto.FirstName != null)
                existingUser.FirstName = updateUserDto.FirstName;
            if (updateUserDto.LastName != null)
                existingUser.LastName = updateUserDto.LastName;
            if (updateUserDto.Email != null)
                existingUser.Email = updateUserDto.Email;
            if (updateUserDto.PhoneNumber != null)
                existingUser.PhoneNumber = updateUserDto.PhoneNumber;

            var updatedUser = await _userService.UpdateUserAsync(id, existingUser);
            if (updatedUser == null)
                return BadRequest("Kullanıcı güncellenemedi.");

            var userDto = new UserResponseDTO
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                PhoneNumber = updatedUser.PhoneNumber,
                CreatedAt = updatedUser.CreatedAt
            };

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDTO>> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _userService.ValidateUserCredentialsAsync(loginDto.Email, loginDto.Password);
            if (!isValid)
                return Unauthorized("Geçersiz email veya şifre.");

            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            var userDto = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }
    }
}
