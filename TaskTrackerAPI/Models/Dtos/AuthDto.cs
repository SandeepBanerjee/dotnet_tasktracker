using System.ComponentModel.DataAnnotations;

namespace TaskTrackerAPI.Models.Dtos
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
    public class AssignRoleRequestDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }

    public class UserDto
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }

        public IEnumerable<string> Roles { get; set; } = System.Array.Empty<string>();
    }
}
