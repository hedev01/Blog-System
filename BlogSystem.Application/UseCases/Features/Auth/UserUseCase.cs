using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Application.DTO.Auth;
using BlogSystem.Application.DTO.User;
using BlogSystem.Domian.Entities;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Domian.Model;
using BlogSystem.Infrastructure.Data;
using BlogSystem.Infrastructure.Services;
using FluentValidation;

namespace BlogSystem.Application.UseCases.Features.Auth
{
    public class UserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public UserUseCase(IUserRepository userRepository, ApplicationDbContext context, IValidator<RegisterRequest> registerValidator, IValidator<LoginRequest> loginValidator, IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;

        }

        public async Task<Result<AuthResponse>> Register(RegisterRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var resultValidator = await _registerValidator.ValidateAsync(request);
                if (!resultValidator.IsValid)
                {
                    var errors = string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage));
                    return Result<AuthResponse>.Failure(errors);
                }

                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var body = new User(request.Username, request.Email, hashPassword, request.FirstName, request.LastName);


                var user = await _userRepository.Register(body);
                if (user == null) return Result<AuthResponse>.Failure("نام کاربری قبلا ثبت شده است.");
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                var refreshTokenEntity = new Domian.Entities.RefreshToken()
                {
                    Token = refreshToken,
                    UserId = user.PublicId,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);

                await transaction.CommitAsync();
                string token = _jwtTokenService.GenerateToken(request.Username);
                var response = new AuthResponse()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
                return Result<AuthResponse>.Success(response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<AuthResponse>.Failure(e.Message);
            }
        }

        public async Task<Result<AuthResponse>> Login(LoginRequest request)
        {
            try
            {
                var resultValidator = await _loginValidator.ValidateAsync(request);
                if (!resultValidator.IsValid)
                {
                    var errors = string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage));
                    return Result<AuthResponse>.Failure(errors);
                }


                var login = await _userRepository.Login(request.username, request.password);
                if (login == null) return Result<AuthResponse>.Failure("نام کاربری یا کلمه عبور اشتباه است.");
                string token = _jwtTokenService.GenerateToken(login.Username);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                var refreshTokenEntity = new Domian.Entities.RefreshToken()
                {
                    Token = refreshToken,
                    UserId = login.PublicId,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                var response = new AuthResponse()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
                return Result<AuthResponse>.Success(response);
            }
            catch (Exception e)
            {
                return Result<AuthResponse>.Failure(e.Message);
            }
        }
    }
}
