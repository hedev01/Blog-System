using BlogSystem.Application.UseCases.Features.RefreshToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly RefreshTokenUseCase _refreshTokenUseCase;
        public RefreshTokenController(RefreshTokenUseCase refreshTokenUseCase)
        {
            _refreshTokenUseCase = refreshTokenUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> NewToken([FromBody] string refreshToken)
        {
            var result = await _refreshTokenUseCase.NewRefreshToken(refreshToken);
            if (result.IsSuccess!.Value) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
    }
}
