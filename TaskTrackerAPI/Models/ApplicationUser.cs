using Microsoft.AspNetCore.Identity;

namespace TaskTrackerAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
