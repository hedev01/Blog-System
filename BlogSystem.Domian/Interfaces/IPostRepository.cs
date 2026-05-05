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
    }
}
