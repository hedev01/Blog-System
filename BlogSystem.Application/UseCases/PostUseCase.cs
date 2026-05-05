using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using BlogSystem.Application.DTO;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;
using BlogSystem.Infrastructure.Data;

namespace BlogSystem.Application.UseCases
{
    public class PostUseCase
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ApplicationDbContext _context;

        public PostUseCase(IPostRepository postRepository, ITagRepository tagRepository , ApplicationDbContext context)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _context = context;
        }

        public async Task<Result<CreatePostResponse>> ExecuteAsync(CreatePostRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Validate(request);
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
                    var tag = await _tagRepository.GetOrCreateByNameAsync(tagName);
                    tagEntities.Add(tag);
                }

                await _tagRepository.AssignTagsToPostAsync(post.Id, tagEntities.Select(t => t.Id).ToList());

                await transaction.CommitAsync();
                //post.Id, post.Title, post.Slug, post.Status, post.CreatedAt,
                //tagEntities.Select(t => t.Name

                var Response = new CreatePostResponse()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    Status = post.Status,
                    CreatedAt = post.CreatedAt,
                    Tags = tagEntities.Select(t =>  t.Name).ToList()
                };

                return Result<CreatePostResponse>.Success(Response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Result<GetPostResponse>> GetPostAsync(ListPostsQuery request)
        {
            var posts = await _postRepository.GetPostAsync(
                request.PageNumber,
                request.PageSize,
                request.AuthorId,
                request.SortOrder);

            var totalCount = await _postRepository.CountPostsAsync(
                request.AuthorId
              );

            var postDtos = posts.Select(p => new Post(p.Title, p.Content, p.CoverImageUrl, p.Status, p.AuthorId))
                .ToList();

            var pagedResult = new PagedResult<Post>
            {
                Items = postDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result<GetPostResponse>.Success(
                new GetPostResponse(pagedResult)
            );
        }



        private void Validate(CreatePostRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Title))
                throw new ArgumentException("Title is required");

            if (string.IsNullOrWhiteSpace(req.Content))
                throw new ArgumentException("Content is required");

            if (req.Title.Length > 150)
                throw new ArgumentException("Title is too long");

            if (req.Status != "draft" && req.Status != "published")
                throw new ArgumentException("Invalid status");

            if (req.Tags.Count > 10)
                throw new ArgumentException("Max 10 tags allowed");

            if (!string.IsNullOrEmpty(req.CoverImageUrl) &&
                !Uri.IsWellFormedUriString(req.CoverImageUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid coverImageUrl");
            }
        }
    }
}
