using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Domian.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> Add(Comment entity);
        Task<List<Comment>> Get(int postId);
    }
}
