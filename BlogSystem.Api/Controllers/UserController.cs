using BlogSystem.Application.DTO.Auth;
using BlogSystem.Application.DTO.User;
using BlogSystem.Application.UseCases.Features.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserUseCase _userCase;

        public UserController(UserUseCase userCase)
        {
            _userCase = userCase;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userCase.Register(request);

            if (result.Value.IsNullOrEmpty()) return BadRequest("متاسفانه کاربر جدید ساخته نشد.");

            return Ok(result);

        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginRequest request)
        {
            var result = await _userCase.Login(request);
            if (result.ErrorMessage != null) return BadRequest(result.ErrorMessage);
            if (result.Value.IsNullOrEmpty()) return BadRequest("متاسفانه توکن ساخته نشد.");
            return Ok(result);
        }
    }
}
