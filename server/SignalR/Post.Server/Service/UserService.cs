using Newtonsoft.Json;
using Post.Server.Models.Dto;
using Post.Server.Service.IService;

namespace Post.Server.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ApplicationUserDto>> GetUsers()
        {
            var client = _httpClientFactory.CreateClient("ApplicationUser");
            var response = await client.GetAsync($"/api/auth/GetUsers");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ApplicationUserDto>>(Convert.ToString(resp.Result));
            }
            return new List<ApplicationUserDto>();
        }
    }
}
