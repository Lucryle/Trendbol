using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
                return BadRequest("Ad alanı boş olamaz.");

            if (string.IsNullOrWhiteSpace(user.LastName))
                return BadRequest("Soyad alanı boş olamaz.");

            if (string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("Email alanı boş olamaz.");

            if (!new EmailAddressAttribute().IsValid(user.Email))
                return BadRequest("Geçerli bir email adresi giriniz.");

            if (string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Şifre alanı boş olamaz.");

            if (user.Password.Length < 6)
                return BadRequest("Şifre en az 6 karakter olmalıdır.");

            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
                return BadRequest("Telefon numarası boş olamaz.");

            if (!new PhoneAttribute().IsValid(user.PhoneNumber))
                return BadRequest("Geçerli bir telefon numarası giriniz.");

            var createdUser = await _userService.CreateUserAsync(user);
            
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Id = id;
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
                return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");

            return NoContent();
        }
    }
}
