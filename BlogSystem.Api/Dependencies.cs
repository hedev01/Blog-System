using BlogSystem.Application.DTO.Auth;
using BlogSystem.Application.DTO.Features.Posts;
using BlogSystem.Application.UseCases.Features.Auth;
using BlogSystem.Application.UseCases.Features.Posts;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Repositories;
using BlogSystem.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.Api
{
    public class Dependencies
    {
       public static void Inject(IServiceCollection service)
        {
            service.AddScoped<IPostRepository, PostRepository>()
                .AddScoped<ITagRepository, TagRepository>()
                .AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>()
                .AddScoped<IValidator<ListPostsRequest>, ListPostsQueryValidator>()
                .AddScoped<IJwtTokenService, JwtTokenService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>()
                .AddScoped<PostUseCase>()
                .AddScoped<UserUseCase>();
        }
    }
}
