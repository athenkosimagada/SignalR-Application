using Auth.Server.Models.Dto;

namespace Auth.Server.Service.IService
{
    public interface IMessageService
    {
        Task<IEnumerable<UserMessageDto>> GetMessages();
    }
}
