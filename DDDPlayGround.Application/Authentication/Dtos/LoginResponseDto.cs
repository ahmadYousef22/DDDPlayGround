
namespace DDDPlayGround.Application.Authentication.Dtos
{
    public class LoginResponseDto
    {
        public string Username { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
