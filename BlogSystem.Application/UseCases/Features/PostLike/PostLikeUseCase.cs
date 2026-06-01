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
                bool findPost = _postRepository.PostExists(request.PostId);
                if (findPost == false) return Result<PostLikeResponse>.Failure("این پست وجود ندارد");
                var existingLike =
                    await _postLikeRepository.GetByUserAndPost(
                        request.UserId,
                        request.PostId);

                PostLikes likeEntity;

                if (existingLike is null)
                {
                    likeEntity = new PostLikes(
                        request.Like,
                        request.UserId,
                        request.PostId);

                    await _postLikeRepository.Like(likeEntity);
                }
                else
                {
                    existingLike.ChangeLike(request.Like);

                    await _postLikeRepository.Update(existingLike);

                    likeEntity = existingLike;
                }

                var response = new PostLikeResponse
                {
                    UserId = likeEntity.UserId,
                    PostId = likeEntity.PostId,
                    Like = likeEntity.Like
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
