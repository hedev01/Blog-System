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
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> GetOrCreateByNameAsync(string name)

        {
            var tag = await _context.Tags.FirstOrDefaultAsync(entity => entity.Name == name);
            if (tag == null)
            {
                tag = new Tag(name);
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;

        }

        public async Task AssignTagsToPostAsync(int postId, List<int> tagIds)
        {
            var post = await _context.Posts.FindAsync(postId) ?? throw new Exception("Post Not Found");

            var tags = await _context.Tags.Where(entity => tagIds.Contains(entity.Id)).ToListAsync();

            await _context.Entry(post).Collection(p => p.Tags).LoadAsync();
            foreach (var tag in tags)
                post.Tags.Add(tag);

            await _context.SaveChangesAsync();

        }
    }
}
