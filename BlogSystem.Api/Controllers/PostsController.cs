using BlogSystem.Application.DTO;
using BlogSystem.Application.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostUseCase _useCase;
        public PostsController(PostUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostRequest request)
        {
            var result = await _useCase.ExecuteAsync(request);
            return Ok(result);
        }
    }
}
