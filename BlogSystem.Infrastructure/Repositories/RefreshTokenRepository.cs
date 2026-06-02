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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RefreshToken entity)
        {
            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> ValidRefreshToken(string refreshToken)
        {
            var result = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
            return result ?? null;
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = await  _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token && r.IsRevoked == false);
            if (refreshToken is not null)
            {
                refreshToken.IsRevoked = true;
               await _context.SaveChangesAsync();
            }
        }
    }
}
