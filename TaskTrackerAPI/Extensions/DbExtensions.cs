using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;

namespace TaskTrackerAPI.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                )
            );

            return services;
        }
    }
}
