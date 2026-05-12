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
        public UserUseCase(IUserRepository userRepository, ApplicationDbContext context, IValidator<RegisterRequest> registerValidator, IValidator<LoginRequest> loginValidator, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _context = context;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<string>> Register(RegisterRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var resultValidator = await _registerValidator.ValidateAsync(request);
                if (!resultValidator.IsValid)
                {
                    var errors = string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage));
                    return Result<string>.Failure(errors);
                }

                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var body = new User(request.Username, request.Email, hashPassword, request.FirstName, request.LastName);


                await _userRepository.Register(body);
                await transaction.CommitAsync();
                string token = _jwtTokenService.GenerateToken(request.Username);
                return Result<string>.Success(token);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result<string>.Failure(e.Message);
            }
        }

        public async Task<Result<string>> Login(LoginRequest request)
        {
            try
            {
                var resultValidator = await _loginValidator.ValidateAsync(request);
                if (!resultValidator.IsValid)
                {
                    var errors = string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage));
                    return Result<string>.Failure(errors);
                }


                var login = await _userRepository.Login(request.username, request.password);
                if (login == null) return Result<string>.Failure("نام کاربری یا کلمه عبور اشتباه است.");
                string token = _jwtTokenService.GenerateToken(login.Username);
                return Result<string>.Success(token);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(e.Message);
            }
        }
    }
}
