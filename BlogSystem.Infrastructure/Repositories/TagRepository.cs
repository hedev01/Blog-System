using System;
using System.Collections;
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
        public async Task<Tag> GetOrCreateByNameAsync(string name , int postId)

        {
            var tag = await _context.Tags.FirstOrDefaultAsync(entity => entity.Name == name);
            if (tag == null)
            {
                tag = new Tag(name , postId);
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;

        }

        public async Task AssignTagsToPostAsync(int postId, List<int> tagIds)
        {
            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == postId) ?? throw new Exception("Post Not Found");

            var tags = await _context.Tags.Where(entity => tagIds.Contains(entity.Id)).ToListAsync();

            foreach (var tag in tags)
                post.Tags.Add(tag);

            await _context.SaveChangesAsync();

        }

        public List<string> GetPostTags(int postId)
        {
            var tagResult = new List<string>();
            var tags = _context.Tags.Where(t => t.PostId == postId).ToList();
            foreach (var items in tags)
            {
                tagResult.Add(items.Name);
            }
            return tagResult;
        }

        public async Task<List<string>> UpdateTags(int postId, List<string> tags)
        {
            var existingTags = await _context.Tags
                .Where(t => t.PostId == postId)
                .ToListAsync();


            foreach (var oldTag in existingTags)
            {
                if (!tags.Contains(oldTag.Name))
                    _context.Tags.Remove(oldTag);
            }


            foreach (var tagName in tags)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name == tagName);

                if (tag == null)
                {

                    _context.Tags.Add(new Tag(tagName , postId));
                }
                else
                {
                    tag.Rename(tagName);
                }
            }

            await _context.SaveChangesAsync();
            return tags;
        }

        public async Task<bool> DeleteTag(int id)
        {
            var findTagById = await _context.Tags.FindAsync(id);
            if (findTagById == null) return false;
            _context.Tags.Remove(findTagById);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
