using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Domian.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Register(User entity);
        Task<User?> Login(string username, string password);
    }
}
