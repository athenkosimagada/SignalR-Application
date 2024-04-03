using Auth.Server.Models.Dto;
using Auth.Server.Service.IService;
using Newtonsoft.Json;

namespace Auth.Server.Service
{
    public class MessageService : IMessageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MessageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<UserMessageDto>> GetMessages()
        {
            var client = _httpClientFactory.CreateClient("Message");
            var response = await client.GetAsync($"/api/message/GetMessages");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<UserMessageDto>>(Convert.ToString(resp.Result));
            }
            return new List<UserMessageDto>();
        }
    }
}
