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
using StackExchange.Redis;
using System.Text.Json;

namespace BlogSystem.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostRepository> _logger;
        private readonly IDatabase _redis;

        public PostRepository(ApplicationDbContext context, ILogger<PostRepository> logger , IConnectionMultiplexer redis)
        {
            _context = context;
            _logger = logger;
            _redis = redis.GetDatabase();
        }
        public async Task<Post> Create(Post entity)

        {

            _context.Posts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;


        }


        public async Task<IReadOnlyList<Post>> GetPostAsync(
            int pageNumber,
            int pageSize,
            Guid authorId,
            string sortOrder)
        {
            var cacheKey = GetCacheKey(pageNumber, pageSize, authorId, sortOrder);

            // 1. Check Redis first
            var cachedData = await _redis.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                _logger.LogInformation("Cache HIT for {CacheKey}", cacheKey);

                return JsonSerializer.Deserialize<List<Post>>(cachedData);
            }

            _logger.LogInformation("Cache MISS for {CacheKey}", cacheKey);

            // 2. DB query
            IQueryable<Post> query = _context.Posts;

            if (authorId != Guid.Empty)
                query = query.Where(p => p.AuthorId == authorId);

            query = sortOrder == "desc"
                ? query.OrderByDescending(p => p.Id)
                : query.OrderBy(p => p.Id);

            var result = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 3. Store in Redis (TTL = 2 minutes)
            var json = JsonSerializer.Serialize(result);

            await _redis.StringSetAsync(
                cacheKey,
                json,
                TimeSpan.FromMinutes(2)
            );

            return result;
        }


        //public async Task<int> CountPostsAsync(int tagId)
        //{
        //    IQueryable<Post> query = _context.Posts.AsNoTracking();

        //    if (tagId > 0)
        //    {
        //        query = query.Where(post => post.Tags.Any(tag => tag.Id == tagId));
        //    }

        //    return await query.CountAsync();
        //}

        public async Task<Post> Update(Post entity, int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                post.Edit(
                    entity.Title,
                    entity.Content,
                    entity.CoverImageUrl,
                    entity.Status
                    );
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var findPostById = await _context.Posts.FindAsync(id);
            if (findPostById == null) return false;
            _context.Posts.Remove(findPostById);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckPostIsValidUser(Guid authorId, int id)
        {
            _logger.LogInformation("Check Post Is Valid UserId {authorId}", authorId);
            var result = await _context.Posts.FirstOrDefaultAsync(p => p.AuthorId == authorId && p.Id == id);
            _logger.LogInformation("User Status: {result}", result);
            return result != null;
        }

        public bool PostExists(int postId)
        {
            var result = _context.Posts.Any(p => p.Id == postId);
            return result;
        }
        private string GetCacheKey(int pageNumber, int pageSize, Guid authorId, string sortOrder)
        {
            return $"posts:{pageNumber}:{pageSize}:{authorId}:{sortOrder}";
        }
    }
}
