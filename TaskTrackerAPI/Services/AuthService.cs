using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;
using TaskTrackerAPI.Models.Dtos;
using TaskTrackerAPI.Services.Interfaces;

namespace TaskTrackerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            AppDbContext appDbContext, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator
            )
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }


        public async Task<UserDto?> Register(RegistrationRequestDto request)
        {
            ApplicationUser user = new()
            {
                UserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email?.ToUpperInvariant(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
            };

            try
            {
                var phoneExists = await _appDbContext
                                        .ApplicationUser
                                        .AnyAsync(u => u.PhoneNumber == request.PhoneNumber);

                if (phoneExists)
                {
                    throw new InvalidOperationException("Phone number is already registered.");
                }


                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(errors);
                }

                var createdUser = await _userManager.FindByNameAsync(user.Email);


                //assign role to user
                if (!string.IsNullOrWhiteSpace(request.Role))
                {
                    var roleName = request.Role.Trim();

                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var createdRoleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                        if (!createdRoleResult.Succeeded)
                        {
                            var rErr = string.Join(", ", createdRoleResult.Errors.Select(e => e.Description));
                            throw new InvalidOperationException($"Failed to create role '{roleName}': {rErr}");
                        }
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(createdUser, roleName);
                    if (!addRoleResult.Succeeded)
                    {
                        var rErr = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to assign role '{roleName}': {rErr}");
                    }
                }

                if (result.Succeeded)
                {
                    var userToReturn = _appDbContext.ApplicationUser.First(u => u.UserName == request.Email);

                    UserDto userDto = new UserDto()
                    {
                        Id = createdUser.Id,
                        FullName = createdUser.FullName,
                        Email = createdUser.Email,
                        PhoneNumber = createdUser.PhoneNumber,
                        Role = (await _userManager.GetRolesAsync(createdUser)).FirstOrDefault() ?? string.Empty,
                        Roles = await _userManager.GetRolesAsync(createdUser)
                    };
                    return userDto;
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto request)
        {
            var userData = await _appDbContext.ApplicationUser.FirstOrDefaultAsync( user => user.UserName.ToLower() == request.UserName.ToLower() );

            if (userData == null)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(userData, request.Password);

            if (isValidPassword == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(userData);
            var token = _jwtTokenGenerator.GenerateToken(userData, roles);

            UserDto userDto = new()
            {
                Email = userData.Email,
                Id = userData.Id,
                FullName = userData.FullName,
                PhoneNumber = userData.PhoneNumber,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Roles = roles
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<UserDto?> AssignRole(string useremail, string userrole)
        {
            var user = await _appDbContext.ApplicationUser.FirstOrDefaultAsync( user => user.Email == useremail );

            if (user != null)
            {
                var checkRoleExists = _roleManager.RoleExistsAsync(userrole).GetAwaiter().GetResult();
                if (checkRoleExists)
                {
                    _roleManager.CreateAsync(new IdentityRole(userrole)).GetAwaiter().GetResult();
                }
                var addRoleResult = await _userManager.AddToRoleAsync(user, userrole);
                if (!addRoleResult.Succeeded)
                {
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);

                return new()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? string.Empty,
                    Roles = roles
                };
            }

            return null;
        }
    }
}
