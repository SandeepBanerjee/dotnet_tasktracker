using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken( ApplicationUser applicationUser, IEnumerable<string> roles );
    }
}
