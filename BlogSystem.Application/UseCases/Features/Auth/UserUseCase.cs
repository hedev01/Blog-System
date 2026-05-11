using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Application.DTO.Auth;
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
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IJwtTokenService _jwtTokenService;
        public UserUseCase(IUserRepository userRepository, ApplicationDbContext context, IValidator<RegisterRequest> validator, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _context = context;
            _validator = validator;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<string>> Register(RegisterRequest request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var resultValidator = await _validator.ValidateAsync(request);
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
    }
}
