using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogSystem.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(ApplicationDbContext context, ILogger<PostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Post> Create(Post entity)

        {

            _context.Posts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<IReadOnlyList<Post>> GetPostAsync(int pageNumber, int pageSize, int authorId, string sortOrder)
        {
            _logger.LogInformation(
                "Fetching posts with PageNumber={PageNumber}, PageSize={PageSize}, authorId={AuthorId}, SortOrder={SortOrder}",
                pageNumber, pageSize, authorId, sortOrder);
            IQueryable<Post> query = _context.Posts;

            if (authorId != 0)
                query = query.Where(p => p.AuthorId == authorId);

            query = sortOrder == "desc" ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
            _logger.LogInformation("Sorting posts by Id {SortOrder}", sortOrder);
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            _logger.LogInformation("Applying paging: Skip={Skip}, Take={Take}",
                (pageNumber - 1) * pageSize,
                pageSize);

            var result = await query.ToListAsync();

            _logger.LogInformation("Fetched {Count} posts from database", result.Count);

            return result;

        }

        public async Task<int> CountPostsAsync(int tagId)
        {
            IQueryable<Post> query = _context.Posts.AsNoTracking();

            if (tagId > 0)
            {
                query = query.Where(post => post.Tags.Any(tag => tag.Id == tagId));
            }

            return await query.CountAsync();
        }
    }
}
