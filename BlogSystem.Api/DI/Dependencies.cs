using BlogSystem.Application.DTO.Auth;
using BlogSystem.Application.DTO.Features.Posts;
using BlogSystem.Application.DTO.User;
using BlogSystem.Application.UseCases.Features.Auth;
using BlogSystem.Application.UseCases.Features.Comment;
using BlogSystem.Application.UseCases.Features.PostLike;
using BlogSystem.Application.UseCases.Features.Posts;
using BlogSystem.Application.UseCases.Features.RefreshToken;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Repositories;
using BlogSystem.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BlogSystem.Api.DI
{
    public class Dependencies
    {
        public static void Inject(IServiceCollection service)
        {
            service.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse("localhost:6379", true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            service.AddScoped<IPostRepository, PostRepository>()
                .AddScoped<ITagRepository, TagRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IPostLikeRepository, PostLikeRepository>()
                .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddScoped<IValidator<ListPostsRequest>, ListPostsQueryValidator>()
                .AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>()
                .AddScoped<IValidator<LoginRequest>, LoginRequestValidator>()
                .AddScoped<IJwtTokenService, JwtTokenService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>()
                .AddScoped<PostUseCase>()
                .AddScoped<UserUseCase>()
                .AddScoped<CommentUseCase>()
                .AddScoped<PostLikeUseCase>()
                .AddScoped<RefreshTokenUseCase>();
        }
    }
}
