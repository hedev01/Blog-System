using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Application.DTO.Comment;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;
using BlogSystem.Infrastructure.Data;

namespace BlogSystem.Application.UseCases.Features.Comment
{
    public class CommentUseCase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly ApplicationDbContext _context;

        public CommentUseCase(ICommentRepository commentRepository, IPostRepository postRepository, ApplicationDbContext context)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _context = context;
        }

        public async Task<Result<CommentResponse>> AddAsync(CommentRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = _postRepository.PostExists(request.postId);
                if (result == false) return Result<CommentResponse>.Failure("این پست وجود ندارد");
                var comment = new Domian.Entities.Comment(request.postId, request.UserId, request.comment);
                await _commentRepository.Add(comment);
                await transaction.CommitAsync();
                var response = new CommentResponse()
                {
                    Id = comment.Id,
                    Comment = comment.Content,
                    PostId = comment.PostId
                };
                return Result<CommentResponse>.Success(response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<CommentResponse>.Failure(e.Message);
            }
        }
    }
}
