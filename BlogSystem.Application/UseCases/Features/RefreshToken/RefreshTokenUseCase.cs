using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Application.DTO.RefreshToken;
using BlogSystem.Application.DTO.User;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;
using BlogSystem.Infrastructure.Data;
using BlogSystem.Infrastructure.Services;

namespace BlogSystem.Application.UseCases.Features.RefreshToken
{
    public class RefreshTokenUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ApplicationDbContext _context;

        public RefreshTokenUseCase(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtTokenService jwtTokenService, ApplicationDbContext context)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<Result<RefreshTokenResponse>> NewRefreshToken(string refreshToken)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var findRefreshToken = await _refreshTokenRepository.ValidRefreshToken(refreshToken);

                if (findRefreshToken == null) return Result<RefreshTokenResponse>.Failure("Token یافت نشد.");

                bool checkUserIsValid = await _userRepository.CheckUserValid(findRefreshToken.UserId);

                if (!checkUserIsValid) return Result<RefreshTokenResponse>.Failure("کاربر معتبر نمی باشد.");

                var user = await _userRepository.GetUser(findRefreshToken.UserId);

                if (string.IsNullOrEmpty(user.Username)) return Result<RefreshTokenResponse>.Failure("کاربر یافت نشد.");

                await _refreshTokenRepository.RevokeRefreshToken(refreshToken);


                string accessToken = _jwtTokenService.GenerateToken(user.Username , user.PublicId , user.Role);

                string refToken = _jwtTokenService.GenerateRefreshToken();
                var refreshTokenEntity = new Domian.Entities.RefreshToken()
                {
                    Token = refToken,
                    UserId = findRefreshToken.UserId,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };

                await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                await transaction.CommitAsync();

                var response = new RefreshTokenResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refToken
                };

                return Result<RefreshTokenResponse>.Success(response);

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<RefreshTokenResponse>.Failure(e.Message);
            }
        }
    }
}
