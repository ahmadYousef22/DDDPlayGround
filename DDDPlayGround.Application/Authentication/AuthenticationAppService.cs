using AutoMapper;
using DDDPlayGround.Application.Authentication.JwtToken;
using DDDPlayGround.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DDDPlayGround.Application.Authentication
{
    public class AuthenticationAppService /*: IAuthenticationAppService*/
    {
        private readonly ILogger<AuthenticationAppService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtService;
        private readonly IMapper _mapper;

        public AuthenticationAppService(
            IUnitOfWork unitOfWork,
            ILogger<AuthenticationAppService> logger,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtService,
            IMapper mapper
            )
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        //public async Task<Response<string>> RegisterAsync(RegisterRequestDto request)
        //{
        //    try
        //    {
        //        if (await _userRepository.ExistsAsync(request.Username))
        //        {
        //            return Response<string>.Failure(HttpStatusCode.BadRequest, "Username already exists");
        //        }

        //        var user = _mapper.Map<User>(request);
        //        user.SetRole(UserRole.User);

        //        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
        //        user.SetPassword(new PasswordHash(hashedPassword));

        //        await _userRepository.AddAsync(user);
        //        return Response<string>.SuccessResult(string.Empty, "User registered successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error in Register", ex);
        //        return Response<string>.Failure(HttpStatusCode.InternalError, "An unexpected error occurred");
        //    }
        //}
        //public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        //{
        //    try
        //    {
        //        var user = await _userRepository.GetByUsernameAsync(request.Username);
        //        if (user == null || !user.IsActive)
        //        {
        //            return Response<LoginResponseDto>.Failure(HttpStatusCode.NotAuthenticated, "Invalid credentials");
        //        }

        //        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash.Value, request.Password);
        //        if (verifyResult == PasswordVerificationResult.Failed)
        //        {
        //            return Response<LoginResponseDto>.Failure(HttpStatusCode.NotAuthenticated, "Invalid credentials");
        //        }

        //        var token = _jwtService.GenerateToken(user);

        //        var loginResponse = _mapper.Map<LoginResponseDto>(user);
        //        loginResponse.Token = token;

        //        return Response<LoginResponseDto>.SuccessResult(loginResponse, "Login successful");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error in Login", ex);
        //        return Response<LoginResponseDto>.Failure(HttpStatusCode.InternalError, "An unexpected error occurred");
        //    }
        //}
        //public async Task LogoutAsync(string refreshToken)
        //{
        //    var userToken = await _userTokenRepository.GetByTokenAsync(refreshToken);

        //    if (userToken == null || userToken.IsRevoked || userToken.IsExpired)
        //    {
        //        return;
        //    }

        //    userToken.Revoke(revokedByIp: null, replacedByToken: null);
        //    await _userTokenRepository.UpdateAsync(userToken);
        //}
        //public async Task<Response<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        //{
        //    var existingToken = await _userTokenRepository.GetByTokenAsync(refreshToken);

        //    if (existingToken == null || existingToken.IsRevoked || existingToken.IsExpired)
        //    {
        //        return Response<LoginResponseDto>.Failure(HttpStatusCode.NotAuthenticated, "Invalid or expired refresh token");
        //    }

        //    var user = await _userRepository.GetByIdAsync(existingToken.UserId);
        //    if (user == null || !user.IsActive)
        //    {
        //        return Response<LoginResponseDto>.Failure(HttpStatusCode.NotAuthenticated, "User not found or inactive");
        //    }

        //    // Generate new JWT token and refresh token
        //    var newJwtToken = _jwtService.GenerateToken(user);
        //    var newRefreshToken = GenerateRefreshToken();

        //    // Revoke old refresh token
        //    existingToken.Revoke(revokedByIp: null, replacedByToken: newRefreshToken);

        //    // Save new refresh token entity
        //    var userToken = new UserToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(30), createdByIp: null);
        //    await _userTokenRepository.UpdateAsync(existingToken);
        //    await _userTokenRepository.AddAsync(userToken);

        //    var response = new LoginResponseDto
        //    {
        //        Token = newJwtToken,
        //        RefreshToken = newRefreshToken,
        //        Username = user.Username
        //    };

        //    return Response<LoginResponseDto>.SuccessResult(response, "Token refreshed successfully");
        //}

        //#region Helper
        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[64];
        //    using var rng = RandomNumberGenerator.Create();
        //    rng.GetBytes(randomNumber);
        //    return Convert.ToBase64String(randomNumber);
        //}
        //#endregion
    }
}
