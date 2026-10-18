using TaskTrackerAPI.Models.Dtos;

namespace TaskTrackerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> Register(RegistrationRequestDto request);
        Task<LoginResponseDto?> Login(LoginRequestDto request);
        Task<UserDto?> AssignRole(string useremail, string userrole);
    }
}
