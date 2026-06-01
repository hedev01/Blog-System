using BlogSystem.Application.DTO.PostLike;
using BlogSystem.Application.UseCases.Features.PostLike;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly PostLikeUseCase _useCase;
        public PostLikeController(PostLikeUseCase useCase)
        {
            _useCase = useCase;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(PostLikeRequest request)
        {
            var result = await _useCase.Like(request);
            if (result.IsSuccess!.Value) return Ok(result);
            else return BadRequest(result.ErrorMessage);

        }
    }
}
