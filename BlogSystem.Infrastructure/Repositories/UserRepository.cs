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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> Register(User entity)
        {
            bool result = _context.Users.Any(u => u.Username == entity.Username);
            if (result) return null;
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();


            return entity;
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValidPassword) return null;
            return user;
        }

        public async Task<bool> CheckUserValid(Guid publicId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PublicId == publicId);
            return user != null;
        }

        public async Task<string> GetUserName(Guid publicId)
        {
            var user = await _context.Users.Where(u => u.PublicId == publicId).Select(u => u.Username).SingleOrDefaultAsync();
            return user ?? "";
        }
    }
}
