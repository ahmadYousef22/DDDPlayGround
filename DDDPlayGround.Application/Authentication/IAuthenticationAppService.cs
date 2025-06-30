using DDDPlayGround.Application.Authentication.Dtos;
using DDDPlayGround.Domain.Base;

namespace DDDPlayGround.Application.Authentication
{
    public interface IAuthenticationAppService
    {
        Task<Response<string>> RegisterAsync(RegisterRequestDto request);
        Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto request);
        Task<Response<LoginResponseDto>> RefreshTokenAsync(string refreshToken);    
        Task LogoutAsync(string refreshToken);
    }
}
