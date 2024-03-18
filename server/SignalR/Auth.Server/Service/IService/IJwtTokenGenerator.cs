using Auth.Server.Models;

namespace Auth.Server.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateTokenAsync(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
