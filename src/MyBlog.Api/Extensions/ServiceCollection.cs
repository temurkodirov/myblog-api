using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Repositories;
using MyBlog.Service.Interfaces.Categories;
using MyBlog.Service.Interfaces.Comments;
using MyBlog.Service.Interfaces.Files;
using MyBlog.Service.Interfaces.Notifications;
using MyBlog.Service.Interfaces.Posts;
using MyBlog.Service.Interfaces.User;
using MyBlog.Service.Interfaces.Users;
using MyBlog.Service.Mappers;
using MyBlog.Service.Services.Categories;
using MyBlog.Service.Services.Comments;
using MyBlog.Service.Services.Files;
using MyBlog.Service.Services.Notifications;
using MyBlog.Service.Services.Posts;
using MyBlog.Service.Services.Users;
using System.Text;

namespace MyBlog.Api.Extensions;

public static class ServiceCollection
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthUserService, AuthUserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IFilesService, FilesService>();
        services.AddScoped<IUserTokenService, UserTokenService>();
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommentService, CommentService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
       
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Gym API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }


}
