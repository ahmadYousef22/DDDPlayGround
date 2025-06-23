
namespace DDDPlayGround.Application.Authentication.Dtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; } = default!;
        public string? Email { get; set; }
        public string Password { get; set; } = default!;
    }
}
