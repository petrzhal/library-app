using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Persistence
{
    public static class LibraryDbContextInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            await context.Database.MigrateAsync();

            await SeedDataAsync(context);
        }

        private static async Task SeedDataAsync(LibraryDbContext context)
        {
            if (!context.Set<Role>().Any())
            {
                context.Set<Role>().AddRange(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Set<User>().Any(u => u.Username == "admin"))
            {
                context.Set<User>().Add(new User
                {
                    Username = "admin",
                    Password = "$2a$10$3U6eKqUW2bvLpIbBwNQ46eutGdvcOEGkU4mkBGwp4LackHtNKhhlG",
                    Email = "petrzhal@gmail.com",
                    RoleId = 1
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
