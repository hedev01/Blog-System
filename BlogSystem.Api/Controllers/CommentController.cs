using BlogSystem.Application.DTO.Comment;
using BlogSystem.Application.UseCases.Features.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentUseCase _commentUseCase;

        public CommentController(CommentUseCase commentUseCase)
        {
            _commentUseCase = commentUseCase;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentRequest request)
        {
            var result = await _commentUseCase.AddAsync(request);
            if (result.IsSuccess.GetValueOrDefault())
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetComment(int postId)
        {
            var result = await _commentUseCase.GetAsync(postId);
            return Ok(result);
        }
    }
}
