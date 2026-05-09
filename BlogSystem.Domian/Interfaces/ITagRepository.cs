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
        Task<Tag> GetOrCreateByNameAsync(string name , int postId);
        Task AssignTagsToPostAsync(int postId, List<int> tagIds);
        List<string> GetPostTags(int postId);
        Task<List<string>> UpdateTags(int postId , List<string> tags);
        Task<bool> DeleteTag(int id);
    }
}
