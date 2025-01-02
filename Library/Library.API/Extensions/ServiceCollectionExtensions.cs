using FluentValidation;
using Library.API.Middlewares;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Library.Application.Mappings;
using Library.Application.DTOs.User;
using Library.Application.Validations.Book;
using Library.Application.Validations.User;
using Library.Application.DTOs.Authors;
using Library.Application.Validations.Author;
using Library.Application.Validations.Image;

namespace Library.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(UserLoginRequest).Assembly);
            });

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<BooksListRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<AddBookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteBookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateBookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetBookByIdRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetBookByIsbnRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<AuthorListRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateAuthorRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteAuthorRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<AddAuthorRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<AuthorBooksRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<BorrowBookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ReturnBookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetUsersBorrowedBooksRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetImageRequestValidator>();

            return services;
        }

        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BookProfile).Assembly);
            services.AddAutoMapper(typeof(PageInfoProfile).Assembly);
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            services.AddAuthentication(options =>
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
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            return services;
        }

        public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionMiddleware>();
            return services;
        }
    }

}
