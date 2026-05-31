using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> Add(Comment entity)
        {
            _context.Comments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Comment>> Get(int postId)
        {
            if (postId == 0) return await _context.Comments.AsNoTracking().ToListAsync();
            var result = _context.Comments.AsNoTracking().Where(c => c.PostId == postId);
            return await result.AsNoTracking().ToListAsync();
        }
    }
}
