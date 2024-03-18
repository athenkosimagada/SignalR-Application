using Auth.Server.Models;
using Auth.Server.Models.Dto;

namespace Auth.Server.Service.IService
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> AssignRoleAsync(string email, string roleName);
        Task<IEnumerable<ApplicationUser>> GetUsers();
        ApplicationUserDto? GetUserById(string Id);
        ApplicationUserDto? GetUserByEmail(string email);
        ApplicationUserDto? GetUserByPhoneNumber(string phoneNumber);
    }
}
