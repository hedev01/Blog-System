using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Application.DTO.Comment;
using BlogSystem.Application.DTO.PostLike;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;

namespace BlogSystem.Application.UseCases.Features.PostLike
{
    public class PostLikeUseCase
    {
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostRepository _postRepository;
        public PostLikeUseCase(IPostLikeRepository postLikeRepository, IPostRepository postRepository)
        {
            _postLikeRepository = postLikeRepository;
            _postRepository = postRepository;
        }

        public async Task<Result<PostLikeResponse>> Like(PostLikeRequest request)
        {
            try
            {

                bool result = _postRepository.PostExists(request.PostId);
                if (result == false) return Result<PostLikeResponse>.Failure("این پست وجود ندارد.");
                var like = new PostLikes(request.Like, request.UserId, request.PostId);
                await _postLikeRepository.Like(like);
                var response = new PostLikeResponse()
                {
                    Like = like.Like,
                    PostId = like.PostId,
                    UserId = like.UserId
                };
                return Result<PostLikeResponse>.Success(response);
            }
            catch (Exception e)
            {
                return Result<PostLikeResponse>.Failure(e.Message);
            }
        }

    }
}
