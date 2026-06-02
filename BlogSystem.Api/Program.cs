using System.Text;
using BlogSystem.Api.DI;
using BlogSystem.Api.Extensions;
using BlogSystem.Application.DTO.Auth;
using BlogSystem.Application.DTO.Features.Posts;
using BlogSystem.Application.UseCases.Features.Auth;
using BlogSystem.Application.UseCases.Features.Posts;
using BlogSystem.Domian.Interfaces;
using BlogSystem.Infrastructure.Data;
using BlogSystem.Infrastructure.Repositories;
using BlogSystem.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddValidatorsFromAssemblyContaining<CreatePostRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<CustomHeaderOperationFilter>();
});
Dependencies.Inject(builder.Services);
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option
        .UseSqlServer("Data Source=.;Initial catalog=Blog; Integrated Security=True;trustservercertificate=true;MultipleActiveResultSets=True;");
});
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync("""
                                               {
                                                   "message": "ابتدا وارد حساب کاربری شوید."
                                               }
                                               """);
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync("""
                                               {
                                                   "message": "شما مجاز به انجام این عملیات نیستید."
                                               }
                                               """);
        }
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
