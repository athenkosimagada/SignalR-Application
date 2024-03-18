namespace Auth.Server.Models.Dto
{
    public class LoginResponseDto
    {
        public ApplicationUserDto? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
