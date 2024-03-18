using Post.Server.Models.Dto;

namespace Post.Server.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDto>> GetUsers();
    }
}
