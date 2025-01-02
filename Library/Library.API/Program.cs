using Library.Infrastructure.Persistence;
using Library.API.Middlewares;
using Library.API.Extensions;
using Library.Application.Mappings;
using MediatR;
using Library.Application.Common.Behaviors;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Library.Application.Validations.Author;
using Library.Application.Validations.Book;
using Library.Application.Validations.Image;
using Library.Application.Validations.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy();

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddTransient<ValidationMiddleware>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                PropertyName = e.Key,
                ErrorMessage = e.Value.Errors.Select(err => err.ErrorMessage)
            });

        return new BadRequestObjectResult(new { Errors = errors });
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddValidationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomMiddlewares();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter accessToken"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost3000");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

await LibraryDbContextInitializer.InitializeAsync(app.Services);

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
