using Microsoft.AspNetCore.Identity;
using TaskTrackerAPI.Constants;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddSignInManager()
              .AddDefaultTokenProviders();

            return services;

        }

        public static async Task SeedRolesAsync( this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
            {
                AppRoles.Manager,
                AppRoles.TechLead,
                AppRoles.Developer
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync( new IdentityRole(role) );

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select( e => e.Description));
                        throw new InvalidOperationException($"Failed to create role {role}: {errors}");
                    }
                }
            }
        }
    }
}
