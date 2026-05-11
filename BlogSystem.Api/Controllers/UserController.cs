using BlogSystem.Application.DTO.Auth;
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _userCase.Register(request);
            if (result.Value.IsNullOrEmpty())
            {
                return BadRequest("متاسفانه کاربر جدید ساخته نشد.");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
