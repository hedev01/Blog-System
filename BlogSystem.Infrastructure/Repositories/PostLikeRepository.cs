using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Data;

namespace BlogSystem.Infrastructure.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public PostLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PostLikes> Like(PostLikes entity)
        {
            _context.PostLikes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
