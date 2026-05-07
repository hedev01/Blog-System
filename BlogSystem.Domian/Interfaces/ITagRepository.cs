using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Model;

namespace BlogSystem.Domian.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetOrCreateByNameAsync(string name);
        Task AssignTagsToPostAsync(int postId, List<int> tagIds);
        List<Tag> GetPostTags(int postId);
        Task<List<string>> UpdateTags(int postId , List<string> tags);
    }
}
