using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Model;
namespace BlogSystem.Domian.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> Create(Post entity);
        Task<IReadOnlyList<Post>> GetPostAsync(int pageNumber, int pageSize, int tagId, string sortOrder);
        Task<int> CountPostsAsync(int id);
        Task<Post> Update(Post entity, int id);
        Task<bool> Delete(int id);


    }
}
