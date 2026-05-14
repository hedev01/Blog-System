using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using BlogSystem.Application.DTO;
using BlogSystem.Application.DTO.Features.Posts;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;
using BlogSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace BlogSystem.Application.UseCases.Features.Posts
{
    public class PostUseCase
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreatePostRequest> _validatorCreatePost;
        private readonly IValidator<ListPostsRequest> _validatorListPostsQuery;
        private readonly IUserRepository _userRepository;

        public PostUseCase(IPostRepository postRepository, ITagRepository tagRepository, ApplicationDbContext context, IValidator<CreatePostRequest> validatorCreatePost, IValidator<ListPostsRequest> validatorListPostsQuery, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _context = context;
            _validatorCreatePost = validatorCreatePost;
            _validatorListPostsQuery = validatorListPostsQuery;
            _userRepository = userRepository;

        }

        public async Task<Result<CreatePostResponse>> ExecuteAsync(CreatePostRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _validatorCreatePost.ValidateAsync(request);

                if (!result.IsValid)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                    return Result<CreatePostResponse>.Failure(errors);
                }

                bool userIsValid = await _userRepository.CheckUserValid(request.AuthorId);
                if (userIsValid == false) return Result<CreatePostResponse>.Failure("کاربر یافت نشد.");

                var post = new Post(
                    request.Title,
                    request.Content,
                    request.CoverImageUrl,
                    request.Status,
                    request.AuthorId
                    );

                post = await _postRepository.Create(post);
                var tagEntities = new List<Tag>();

                foreach (var tagName in request.Tags)
                {
                    var tag = await _tagRepository.GetOrCreateByNameAsync(tagName, post.Id);
                    tagEntities.Add(tag);
                }

                await _tagRepository.AssignTagsToPostAsync(post.Id, tagEntities.Select(t => t.Id).ToList());

                await transaction.CommitAsync();

                var Response = new CreatePostResponse()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    Status = post.Status,
                    CreatedAt = post.CreatedAt,
                    Tags = tagEntities.Select(t => t.Name).ToList()
                };

                return Result<CreatePostResponse>.Success(Response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Result<PagedResult<ListPostResponse>>> GetPostAsync(ListPostsRequest request)
        {
            var result = await _validatorListPostsQuery.ValidateAsync(request);
            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                return Result<PagedResult<ListPostResponse>>.Failure(errors);
            }

            var posts = await _postRepository.GetPostAsync(
                request.PageNumber,
                request.PageSize,
                request.AuthorId,
                request.SortOrder);


            var postDtos = posts.Select(p => new ListPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CoverImageUrl = p.CoverImageUrl,
                Status = p.Status,
                AuthorId = p.AuthorId,
                TagName = _tagRepository.GetPostTags(p.Id)
            })
                .ToList();

            var pagedResult = new PagedResult<ListPostResponse>
            {
                Items = postDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = posts.Count
            };

            return Result<PagedResult<ListPostResponse>>.Success(
                pagedResult
            );
        }

        public async Task<Result<UpdatePostResponse>> UpdatePost(UpdatePostRequest request, int postId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userValidForPost = await _postRepository.CheckPostIsValidUser(request.AuthorId , postId);
                if (userValidForPost == false) return Result<UpdatePostResponse>.Failure("شما مجاز به ویرایش این مقاله نیستید.");
                var post = new Post(request.Title, request.Content, request.CoverImageUrl, request.Status, request.AuthorId);
                post = await _postRepository.Update(post, postId);

                var tags = await _tagRepository.UpdateTags(postId, request.Tags);

               // var postTags = _tagRepository.GetPostTags(id);

                await transaction.CommitAsync();

                var response = new UpdatePostResponse()
                {
                    Id = postId,
                    Title = post.Title,
                    Status = post.Status,
                    UpdatedAt = DateTime.UtcNow,
                    Tags = tags
                };
                return Result<UpdatePostResponse>.Success(response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<UpdatePostResponse>.Failure(e.Message);
            }
        }

        public async Task<Result<bool>> DeletePost(int id)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var taskResult = await _postRepository.Delete(id);
                if (taskResult)
                {
                    await _tagRepository.DeleteTag(id);
                }

                await transaction.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<bool>.Failure(e.Message);
            }
        }

    }
}
