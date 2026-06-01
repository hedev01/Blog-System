using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Domian.Interfaces
{
    public interface IPostLikeRepository
    {
        Task<PostLikes> Like(PostLikes entity);
        Task<PostLikes?> GetByUserAndPost(Guid userId, int postId);
        Task Update(PostLikes likes);
    }
}
