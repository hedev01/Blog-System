using BlogSystem.Application.DTO.Features.Posts;
using BlogSystem.Application.UseCases.Features.Posts;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> Create(CreatePostRequest request)
        {
            var result = await _useCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPosts([FromQuery] int pageNumber = 1, [FromQuery] int PageSize = 10, [FromQuery] int AuthorId = 1, [FromQuery] string SortOrder = "desc")
        {
            var result = await _useCase.GetPostAsync(new ListPostsRequest(pageNumber, PageSize, AuthorId, SortOrder));
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest request, [FromQuery] int id)
        {
            var result = await _useCase.UpdatePost(request, id);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _useCase.DeletePost(id);
            return Ok(result);
        }
    }
}
