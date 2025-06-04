using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Services.Interfaces;
using TrendbolAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService userService, IJwtService jwtService, IEmailService emailService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!new EmailAddressAttribute().IsValid(request.Email))
                return BadRequest("Geçersiz email formatı");
            if (await _userService.IsEmailAvailableAsync(request.Email))
            {
                var success = await _emailService.SendVerificationCodeAsync(request.Email);
                if (!success)
                    return StatusCode(500, "Doğrulama kodu gönderilemedi");

                return Ok(new { message = "Doğrulama kodu gönderildi" });
            }

            return BadRequest("Bu email adresi zaten kullanımda");
        }

        [HttpPost("verify-register")]
        public async Task<IActionResult> VerifyRegister([FromBody] VerifyRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _emailService.VerifyCodeAsync(request.Email, request.VerificationCode);
            if (!isValid)
                return BadRequest("Geçersiz veya süresi dolmuş doğrulama kodu");

            var user = await _userService.CreateUserAsync(new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsVerified = true
            });

            if (user == null)
                return StatusCode(500, "Kullanıcı oluşturulamadı");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                message = "Kayıt başarılı",
                token,
                user = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    user.IsVerified
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Geçersiz email veya şifre");

            if (!user.IsVerified)
                return Unauthorized("Email adresiniz doğrulanmamış");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                message = "Giriş başarılı",
                token,
                user = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    user.IsVerified
                }
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            if (user.IsVerified)
                return BadRequest("Email adresi zaten doğrulanmış");

            var success = await _emailService.SendVerificationCodeAsync(request.Email);
            if (!success)
                return StatusCode(500, "Doğrulama kodu gönderilemedi");

            return Ok(new { message = "Doğrulama kodu gönderildi" });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            if (user.IsVerified)
                return BadRequest("Email adresi zaten doğrulanmış");

            var isValid = await _emailService.VerifyCodeAsync(request.Email, request.VerificationCode);
            if (!isValid)
                return BadRequest("Geçersiz veya süresi dolmuş doğrulama kodu");

            user.IsVerified = true;
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
                return StatusCode(500, "Email doğrulanamadı");

            return Ok(new { message = "Email başarıyla doğrulandı" });
        }
    }
} 